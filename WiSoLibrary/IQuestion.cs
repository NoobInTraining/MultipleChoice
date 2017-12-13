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

		QuestionTypes QuestionType { get; }


		bool IsCorrect(params object[] param);
	}
}
