using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	/// <summary>
	/// Represents a final exam
	/// </summary>
	public class Exam
	{
		public Exam()
		{
			Questions = new List<IQuestion>();
		}

		/// <summary>
		/// The name of the exam
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The Year in wich this exam was written in.
		/// </summary>
		public DateTime Year { get; set; }

		/// <summary>
		/// Sets the year Property of this class according to the season and the year it was written in.
		/// </summary>
		/// <remarks>Year = new DateTime(year.Year, (int) season, 1);</remarks>
		public void SetYearFromDateAndSeason(DateTime year, Season season)
		{
			Year = new DateTime(year.Year, (int) season, 1);
		}

		/// <summary>
		/// A List of questions contianed in the exam
		/// </summary>
		public List<IQuestion> Questions { get; private set; }

		/// <summary>
		/// Adds a question to the array
		/// </summary>
		/// <param name="question"></param>
		/// <exception cref="ArgumentNullException"/>
		public void AddQuestion(SimpleQuestion question)
		{
			if (question.Answers == null || question.Answers.Length == 0 || string.IsNullOrWhiteSpace(question.Text))
				throw new ArgumentNullException("Answer and Text of a Question must be set.");

			Questions.Add(question);
		}

		/// <summary>
		/// Parses the xml and returns the Exam with it's questions
		/// </summary>
		/// <param name="path">Path to the exams xml file</param>
		/// <returns>The parsed exam</returns>
		/// <exception cref="NotImplementedException"/>
		public static Exam GetExamFromXML(string path)
		{
			Exam exam = new Exam();
			
			//initilize the xml
			var xml = XDocument.Load(path);

			//set the name
			exam.Name = xml.Root.Attribute("Name").Value;
			var season = xml.Root.Attribute("Season").Value;
			var year = xml.Root.Attribute("Year").Value;
			Enum.Parse(typeof(Season), season, true);

			//itterate through all questions
			foreach (var q in xml.Root.Elements())
			{
				#region [ 13-12-2017: Mutiple type support ]

				//check if "Type" attribute exists
				var attr = q.Attribute("Type");
				if (attr != null)
				{
					//get the type
					switch (QuestionTypes.Assign.FromString(attr.Value))
					{
						case QuestionTypes.Sort:
							parseSortQuestion(q);
							continue;
						case QuestionTypes.Assign:
							continue;
						case QuestionTypes.Input:
							continue;
						//continue by doing it the original way
						default:
							break;
					}
				}

				#endregion

				var answerElements = q.Element("Answers").Elements().ToArray();
				var correctAnswers = q.Element("CorrectAnswers");

				//generate each question
				List<SimpleAnswer> answers = new List<SimpleAnswer>(answerElements.Length);
				Array.ForEach(answerElements, (a) =>
				{
					//add the value of the answer and search the correctAnswers elemnt if it contains it
					answers.Add(new SimpleAnswer(a.Value, correctAnswers.Elements().Count(c => c.Attribute("Number").Value == a.Attribute("Number").Value) > 0));
				});

				exam.AddQuestion(new SimpleQuestion(q.Element("Text").Value, answers.ToArray()));				
			}
			return exam;
		}

		/// <summary>
		/// Parses an element of the questions XML of type Sort
		/// </summary>
		/// <param name="question"></param>
		/// <returns></returns>
		/// <exception cref="Exception">If the Question is not of type Sort.</exception>
		private static SortQuestion parseSortQuestion(XElement question)
		{
			//check if the question is of type Sort
			if (!question.Attribute("Type").Value.Equals("Sort", StringComparison.OrdinalIgnoreCase))
				throw new Exception("Wrong question type");

			//create a temporary list of answers
			List<MatchedAnswer> answers = new List<MatchedAnswer>();

			//Read out the possible answers and the correct answers
			var possibleAnswer = question.Element("Answers").Elements("Answer");
			var correctStep = question.Element("CorrectAnswers").Element("Answer").Attribute("Number").Value.Split(',');

			//check if the numbers match
			if (possibleAnswer.Count() != correctStep.Count())
				throw new Exception("The amount of possible answers and given answers arn't equal.");
			//check if all possible answers where used
			else if (possibleAnswer.All(p => correctStep.Any(b => b == p.Attribute("Number").Value)))
				throw new Exception("Not all answers appear are used.");
			
			//itterate thorugh the correct steps 
			for (int i = 0; i < correctStep.Length; i++)
			{
				//get the correct step for this occasion
				var step = correctStep[i];
				var fitting = possibleAnswer.Single(p => p.Attribute("Number").Value == step);
				//add the step
				answers.Add(new MatchedAnswer(fitting.Value, i + 1));
			}

			//return the question
			return new SortQuestion(question.Element("Text").Value, answers.ToArray());
		}
	}
}
