using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiSoLibrary;

namespace MultipleChoiceConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var d = Exam.GetExamFromXML(@"C:\Users\Mümmelmann\Google Drive\Alle WISO\Korrigierte\16_So_WISO\questions.xml");

			foreach (var question in d.Questions.Randomize())
			{
				Console.WriteLine(question.Text);
				Console.WriteLine();

				var answers = question.Answers.Randomize();
				for (int i = 0; i < answers.Length; i++)
				{
					Console.WriteLine($"\t{i + 1}. {answers[i].Text}");
				}

				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("Type in the number of the correct answer.");

				var cr = Console.ReadKey();
				if (answers[int.Parse(cr.ToString()) - 1].IsCorrect)
					Console.WriteLine("Correct");
				else
					Console.WriteLine("False");

			}
		}
	}
}
