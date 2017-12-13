using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
    public class SimpleQuestion : IQuestion
    {
		/// <summary>
		/// Represents a question
		/// </summary>
		/// <param name="text">The actual question</param>
		/// <param name="answers">List of answers that are accepted.</param>
		public SimpleQuestion(string text, params SimpleAnswer[] answers) : this(text, null, answers)
		{
		}

		/// <summary>
		/// Represents a question
		/// </summary>
		/// <param name="text">The actual question</param>
		/// <param name="imagePath">Path of an image belonging to the exam</param>
		/// <param name="answers">List of answers that are accepted.</param>
		public SimpleQuestion(string text, string imagePath, params SimpleAnswer[] answers)
		{
			Text = text;
			var tmp = new List<IAnswer>(answers.Length);
			Array.ForEach(answers, p => tmp.Add(p));
			Answers = tmp.ToArray();
			ImagePath = imagePath;
			QuestionType = QuestionTypes.Default;
		}
		/// <summary>
		/// The question Text
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// The possible Answers
		/// </summary>
		public IAnswer[] Answers { get; set; }

		/// <summary>
		/// Filter on Answers where IsCorrect is true.
		/// </summary>
		public IEnumerable<IAnswer> CorrectAnswers { get { return Answers.Where(a => ((SimpleAnswer)a).IsCorrect); } }

		/// <summary>
		/// An image belonging to the question
		/// </summary>
		public string ImagePath { get; set; }

		public QuestionTypes QuestionType{ get; }

		string[] IQuestion.CorrectAnswers
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsCorrect(params IAnswer[] param)
		{
			throw new NotImplementedException();
		}
	}
}
