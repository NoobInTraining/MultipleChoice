using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	public struct MatchedAnswer : IAnswer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text">The questiontext</param>
		/// <param name="position">The correct position of this answer</param>
		public MatchedAnswer(string text, int position)
		{
			Text = text;			
			Position = position;
		}
		
		/// <summary>
		/// The Question Text
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// The correct position of this answer
		/// </summary>
		public int Position { get; set; }
	}
}
