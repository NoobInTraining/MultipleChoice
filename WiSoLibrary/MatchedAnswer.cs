using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	public struct MatchedAnswer : IAnswer
	{
		public MatchedAnswer(IAnswer key, IAnswer value)
		{
			this.AnswerPair = new KeyValuePair<IAnswer, IAnswer>(key, value);
		}

		KeyValuePair<IAnswer, IAnswer> AnswerPair { get; }

		public string Text => "A matched Question.";
	}
}
