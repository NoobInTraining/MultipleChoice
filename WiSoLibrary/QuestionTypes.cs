using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	public enum QuestionTypes
	{
		/// <summary>
		/// A Simple multiplechoice question with an variable array of answers
		/// </summary>
		Default,

		/// <summary>
		/// A question where you have to Sort the given values
		/// </summary>
		Sort,

		/// <summary>
		/// A question where you have to assign certain values to eachother
		/// </summary>
		Assign,

		/// <summary>
		/// A Question that requires user input
		/// </summary>
		Input

	}
}
