using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
    public struct Question : IQuestion
    {
		/// <summary>
		/// Represents a question
		/// </summary>
		/// <param name="text">The actual question</param>
		/// <param name="answers">List of answers that are accepted.</param>
		public Question(string text, params Answer[] answers) : this(text, null, answers)
		{
		}

		/// <summary>
		/// Represents a question
		/// </summary>
		/// <param name="text">The actual question</param>
		/// <param name="imagePath">Path of an image belonging to the exam</param>
		/// <param name="answers">List of answers that are accepted.</param>
		public Question(string text, string imagePath, params Answer[] answers)
		{
			Text = text;
			Answers = answers;
			ImagePath = imagePath;
		}
		/// <summary>
		/// The question Text
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// The possible Answers
		/// </summary>
		public Answer[] Answers { get; set; }

		/// <summary>
		/// Filter on Answers where IsCorrect is true.
		/// </summary>
		public IEnumerable<Answer> CorrectAnswers { get { return Answers.Where(a => a.IsCorrect); } }

		/// <summary>
		/// An image belonging to the question
		/// </summary>
		public string ImagePath { get; set; }

		public bool IsCorrect(params object[] param)
		{
			throw new NotImplementedException();
		}
	}
}
