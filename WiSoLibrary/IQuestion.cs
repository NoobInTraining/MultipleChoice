using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{

	/// <summary>
	/// Am interface representing wich each question needs
	/// </summary>
	public interface IQuestion
	{

		/// <summary>
		/// The question Text
		/// </summary>
		string Text { get; }
		
		/// <summary>
		/// The possible Answers
		/// </summary>
		IAnswer[] Answers { get; }

		/// <summary>
		/// The Questiontype of this question
		/// </summary>
		QuestionTypes QuestionType { get; }

		/// <summary>
		/// Checks wether a parameter is correct
		/// </summary>
		/// <param name="param">Answers given by the user</param>
		/// <returns>If given answers are correct</returns>
		bool IsCorrect(params IAnswer[] param);

		/// <summary>
		/// Returns an Array of correct answer
		/// </summary>
		string[] CorrectAnswers { get; }
	}
}
