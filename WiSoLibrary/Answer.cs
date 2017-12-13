using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	/// <summary>
	/// An answer to a quesion
	/// </summary>
	public struct SimpleAnswer : IAnswer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text">The answer</param>
		/// <param name="isCorrect">Flag if it's the right answer</param>
		public SimpleAnswer(string text, bool isCorrect)
		{
			Text = text;
			IsCorrect = isCorrect;
		}

		/// <summary>
		/// The Answertext
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Bollean value to determin wether the Answer is the right answer
		/// </summary>
		public bool IsCorrect { get; set; }
	}
}
