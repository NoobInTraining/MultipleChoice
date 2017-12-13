using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	public class SortQuestion : IQuestion
	{
		/// <summary>
		/// Representing a question of type "Sort"
		/// </summary>
		/// <param name="text"></param>
		public SortQuestion(string text)
		{
			this.Text = text;
			this.QuestionType = QuestionTypes.Sort;
			
		}

		public IAnswer[] Answers
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string[] CorrectAnswers
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public QuestionTypes QuestionType{ get; }

		public string Text { get; }

			
		bool IQuestion.IsCorrect(params IAnswer[] param)
		{
			if (!param.All(p => p is MatchedAnswer))
				throw new ArgumentException("The passed Answers must be of type " + nameof(MatchedAnswer));

			return IsCorrect(Array.ConvertAll(param, p => (MatchedAnswer) p));
		}

		public bool IsCorrect(params MatchedAnswer[] matchedAnswer)
		{
			throw new NotImplementedException();
		}
	}
}
