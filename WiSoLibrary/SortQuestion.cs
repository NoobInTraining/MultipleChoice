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
		/// <param name="answers">Takes in Answers and randomizes them</param>
		public SortQuestion(string text, MatchedAnswer[] answers)
		{
			this.Text = text;
			this.QuestionType = QuestionTypes.Sort;
			this.possibleAnswers = answers.Randomize();
		}

		private MatchedAnswer[] possibleAnswers;

		/// <summary>
		/// Retrieves the Answer Array
		/// </summary>
		public IAnswer[] Answers => Array.ConvertAll(possibleAnswers, p => (IAnswer) p);

		/// <summary>
		/// Sorted anser array in format: "{Position}. {Text}"
		/// </summary>
		public string[] CorrectAnswers => Array.ConvertAll(possibleAnswers.OrderBy(p => p.Position).ToArray(), p => $"{p.Position}. {p.Text}");

		/// <summary>
		/// returns the questiontype
		/// </summary>
		public QuestionTypes QuestionType{ get; }

		/// <summary>
		/// The question text
		/// </summary>
		public string Text { get; }

		///<exception cref="ArgumentException">When not all passed arguments are of type MatchedAnswer</exception>
		/// <exception cref="NotEnoughAnswersGivenException"/>
		/// <exception cref="ArgumentException">If the is an Answer not given or doubble</exception>
		bool IQuestion.IsCorrect(params IAnswer[] param)
		{
			if (!param.All(p => p is MatchedAnswer))
				throw new ArgumentException("The passed Answers must be of type " + nameof(MatchedAnswer));

			return IsCorrect(Array.ConvertAll(param, p => (MatchedAnswer) p));
		}

		/// <summary>
		/// Checked wether the given answer array is correct
		/// </summary>
		/// <param name="matchedAnswer">The answers wich where given my the user with their answer of th eposition</param>
		/// <returns></returns>
		/// <exception cref="NotEnoughAnswersGivenException"/>
		/// <exception cref="ArgumentException">If the is an Answer not given or doubble</exception>
		public bool IsCorrect(params MatchedAnswer[] matchedAnswer)
		{
			//check if the right amount of answers where supplieod
			if (matchedAnswer.Length != possibleAnswers.Length)
				throw new NotEnoughAnswersGivenException($"{(matchedAnswer.Length > possibleAnswers.Length? "Tomany" : "Not enough")} Answers given. " +
														 $"Length needs to match the possible Answers ({possibleAnswers.Length})");
			try
			{
				//check if all answers are the correct 
				return matchedAnswer.All(g => possibleAnswers.Single(m => m.Text == g.Text).Position == g.Position);
			}			
			catch (InvalidOperationException ex)
			{
				throw new ArgumentException("Couldn't locate or doubbled an answer. Refer to inner exception for more deatil.", ex);				
			}
		}
	}
}
