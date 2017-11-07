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
			Questions = new List<Question>();
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
		public List<Question> Questions { get; private set; }

		/// <summary>
		/// Adds a question to the array
		/// </summary>
		/// <param name="question"></param>
		/// <exception cref="ArgumentNullException"/>
		public void AddQuestion(Question question)
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
				var answerElements = q.Element("Answers").Elements().ToArray();
				var correctAnswers = q.Element("CorrectAnswers");
				//generate each question
				List<Answer> answers = new List<Answer>(answerElements.Length);
				Array.ForEach(answerElements, (a) =>
				{
					//add the value of the answer and search the correctAnswers elemnt if it contains it
					answers.Add(new Answer(a.Value, correctAnswers.Elements().Count(c => c.Attribute("Number").Value == a.Attribute("Number").Value) > 0));
				});

				exam.AddQuestion(new Question(q.Element("Text").Value, answers.ToArray()));
				
			}
			return exam;
		}
	}
}
