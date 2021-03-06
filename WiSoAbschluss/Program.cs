﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WiSoAbschluss
{
    class Program
    {
        private static List<Process> processes;
        private static StringBuilder questions;

		static void Main(string[] args)
		{

			string path = @"C:\Users\Mümmelmann\Google Drive\Alle WISO\Als bild"; //<- New directory in drive
					//  @"C:\Users\Mümmelmann\Documents\visual studio 2017\Projects\WiSoAbschluss\PDF24 Conversions"; // <- Old Dirceoty
			if (!Directory.Exists(path))
				/*path = @"D:\Git Projects\MultipleChoice\WISO_W2016_A_PDF24"*/;
			
			foreach(var directory in Directory.GetDirectories(path))
			{
				processDirecotry(directory, false);
			}

		}

		static void processDirecotry(string path, bool convert = true, string fileEnding = ".png")
		{ 
            processes = new List<Process>(13);

			if (convert)
				convertAllImagesInDicrectoryAsync(path, fileEnding);

			questions = new StringBuilder(100);
			questions.Append("<Exam ");
			calculateYearAndSuch(path);


			//as soon as the first process stops we can start working 
			while (!processes.All(p => p.HasExited))
            {
                Thread.Sleep(2000);
                while (processes.Any(p => p.HasExited))
                {
                    //remove all processes that have finished
                    foreach (var p in processes.Where(p => p.HasExited).ToList())
                        processes.Remove(p);

                    //start converting textfiles
                    convertTextFiles(path);
                }
            }

            convertTextFiles(path);

            questions.AppendLine("</Exam>");
            File.WriteAllText($"{path}\\questions.xml", questions.ToString(), Encoding.UTF8);

        }

		/// <summary>
		/// Adds the Year And Season attribbute wich is derived from th eifle header
		/// </summary>
		/// <param name="path"></param>
		private static void calculateYearAndSuch(string path)
		{
			var myear = Regex.Match(path, @"\d\d_");
			var mseason = Regex.Match(path, @"[WS][io]");

			string year = "20" + myear.Value.Replace("_", ""); 
			string season = mseason.Value == "Wi" ? "Winter" : "Summer";
			questions.AppendLine($"Name=\"{Path.GetFileName(path)}\" Year=\"{year}\" Season=\"{season}\">");			
		}		

		/// <summary>
		/// Yoloconverts textfiles to xml
		/// </summary>
		/// <param name="path"></param>
		private static void convertTextFiles(string path)
        {
            var answer = new StringBuilder();
            bool isAnswer = false;
			bool foundAtleastOneQuestion = false, isNotFirstQuestion = false, alreadyClosed = false;
			bool continueToNextQuestion = false;
			string[] ignoreLines = new string[] {
				"ZPA IT WiSo" ,
				"PRÜFUNGSZEIT — NICHT ",
				"Wie beurteilen Sie nach der Bearbeitung der Aufgaben die zur Verfügung stehend",
				"Sie hätte kürzer sein können. Sie war angemessen"
			};
			//itterate trhourh all ifles
			foreach (var file in Directory.GetFiles(path, "*.txt"))                
            {
                int answerNumber = 1;

				try
				{
					//ittreate thorugh all ines in that file
					foreach (string line in File.ReadAllLines(file))
					{
						#region [ start ofa new question ]

						if (ignoreLines.Any(p => line.StartsWith(p)))
							continue;

						//check if we shall continue to next question
						if (continueToNextQuestion && !Regex.IsMatch(line, @"^\d+\.? Aufgabe"))
							continue;

						//the begiinig of a new question
						if (Regex.IsMatch(line, @"^\d+\.? Aufgabe"))
						{
							//close the other quesion
							if (isNotFirstQuestion || !(questions.ToString().EndsWith("Season=\"Summer\">\r\n") || questions.ToString().EndsWith("Season=\"Winter\">\r\n")))
							{
								//check if question closed already
								if (!alreadyClosed)
								{
									questions.AppendLine("</Answers>");
									questions.AppendLine("<CorrectAnswers>");
									questions.AppendLine("\t<!--TODO: Mark correct answers-->");
									questions.AppendLine("\t<Answer Number=\"\"/>");
									questions.AppendLine("</CorrectAnswers>");
								}

								//append lines to mark the begiinin g of anew quesion
								questions.AppendLine("</Question>");

								//reset answer counter
								answerNumber = 1;

								//found atleast on quesiton
								isNotFirstQuestion = true;
								continueToNextQuestion = false;
								alreadyClosed = false;
							}

							//Open the next question
							questions.AppendLine($"<Question Number=\"{Regex.Match(line, "\\d+").Value}\">");
							questions.Append("<Text><![CDATA[");

							//not the answer block no more
							isAnswer = false;
							//question was writte
							foundAtleastOneQuestion = true;
						}

						#endregion [ start ofa new question ]

						//having issues with "Situation" at the begiining of an exam --> when first question is registered this wont be executed anymore
						if (!foundAtleastOneQuestion)
							continue;

						//don't work on any empty lines
						if (!string.IsNullOrWhiteSpace(line))
						{
							//during answerblock
							if (isAnswer)
							{								
								//part of the answer								
								answer.Append(line + " ");

								//end of this answer
								if (lineIsAnswerEnding(line))
								{
									questions.Append(answer.ToString().Trim());
									questions.AppendLine("]]></Answer>");

									//clear current answer and start new answer
									answer.Clear();
									answer.Append($"\t<Answer Number=\"{answerNumber++}\"><![CDATA[");
								}
							}
							//apppend the line if its not aufgabe xx
							else
								if(!Regex.IsMatch(line, @"^\d+\.? Aufgabe"))
									questions.AppendLine(line);
						}

						#region [ Check if the next block is the answer block ]

						//the start of an answer block
						if (Regex.IsMatch(line, "T?ragen Sie (\\w )*die Ziffern?"))
						{
							//answer block starts
							isAnswer = true;
							answer.Clear();
							//close the text
							questions.AppendLine("]]></Text>");
							//start the answers block
							questions.AppendLine("<Answers>");
							answer.Append($"<Answer Number=\"{answerNumber++}\"><![CDATA[");
						}

						#region [ check if we shall commence to end due to incopabilities]
						//thesse types of quesions arnt supported
						if (Regex.IsMatch(line, "T?ragen Sie (\\w )*(die Anzahl)|(das ermittelte Datum)|(das Ergebnis)|(die Ergebnisse)"))
						{

							//answer block starts
							isAnswer = true;
							answer.Clear();
							//close the text
							questions.AppendLine();
							questions.AppendLine("::Not Supported::");
							questions.AppendLine("]]></Text>");
							//start the answers block
							questions.AppendLine("<!-- TODO: Question Support -->");
							questions.AppendLine("<Answers>");							
							questions.AppendLine($"<Answer Number=\"{answerNumber}\">Not Supported</Answer>");
							questions.AppendLine("</Answers>");
							questions.AppendLine("<CorrectAnswers>");
							questions.AppendLine($"\t<Answer Number=\"{answerNumber}\"/>");
							questions.AppendLine("</CorrectAnswers>");

							alreadyClosed = true;
							continueToNextQuestion = true;
						}
						#endregion

						#endregion [ Check if the next block is the answer block ]
					}

					//delete the file
					//File.Delete(file);
				}
				//kann sein dass datei benützt wird
				catch (System.IO.IOException)
				{


				}
            }

			//The last question is closed here
			questions.AppendLine("</Answers>");
			questions.AppendLine("<CorrectAnswers>");
			questions.AppendLine("\t<!--TODO: Mark correct answers-->");
			questions.AppendLine("\t<Answer Number=\"\"/>");
			questions.AppendLine("</CorrectAnswers>");
			questions.AppendLine("</Question>");
		}

		/// <summary>
		/// checks wether line cotains less than 7 words -> new 
		/// or if it has a sentence ending.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		private static bool lineIsAnswerEnding(string line)
		{			
			//assumption if under9 words its the end
			if (line.Split(' ').Length <= 9)
				return true;

			//If the lines ends on either . or ? or !
			return Regex.IsMatch(line, "[!.?]$");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ending">The flye ending</param>
		private static void convertAllImagesInDicrectoryAsync(string directory, string ending = ".jpg")
        {                    
            //Convert all images
            foreach (var file in Directory.GetFiles(directory, $"*{ending}"))
            {
                callConvertAsync(file);
                Thread.Sleep(50);
            }
        }

        private static async void callConvertAsync(string file)
        {
            await convertFileAsync(file);
        }

        /// <summary>
        /// Spawns a process and parses the file with tesseract
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static Task convertFileAsync(string file)
        {
            return Task.Factory.StartNew(() =>
            {                               
                var tesspath = @"D:\Tesseract\Tesseract-OCR\tesseract.exe";
                var tessdata = @"D:\Tesseract\Tesseract-OCR\tessdata";

                var arguemtns = "\"" + file + "\" \"" + Regex.Replace(file, "\\.\\w+", ".txt") + "\" --tessdata-dir \"" + tessdata + "\" -l deu";
                
                Process p = new Process();
                p.StartInfo.Arguments = arguemtns;
                p.StartInfo.FileName = tesspath;
                p.Start();

                processes.Add(p);
            });
        }
    }
}
