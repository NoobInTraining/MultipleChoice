using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	public class Exam
	{
		public Exam()
		{
			Questions = new List<Question>();
		}

		public string Name { get; set; }

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
