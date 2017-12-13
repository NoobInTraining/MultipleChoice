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

	public static class QuestionTypesExtension
	{
		/// <summary>
		/// Converts from Enum to string
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string Format(this QuestionTypes type)
		{
			switch (type)
			{
				case QuestionTypes.Default:	return "Default";
				case QuestionTypes.Sort:	return "Sort";
				case QuestionTypes.Assign:	return "Assign";
				case QuestionTypes.Input:	return "Input";
				default: throw new NotSupportedException("The passed QuestionType is not supported: " + nameof(type));
			}
		}

		/// <summary>
		/// Converts from string to enum
		/// </summary>
		/// <param name="type"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static QuestionTypes FromString(this QuestionTypes type, string s)
		{
			switch (s)
			{
				case "Default": return QuestionTypes.Default;
				case "Sort":	return QuestionTypes.Sort	;
				case "Assign":	return QuestionTypes.Assign	;
				case "Input":	return QuestionTypes.Input	;
				default: throw new NotSupportedException("Converson from string \"" + s + "\" is not supported.");
			}
		}
	}
		
}
