using Microsoft.VisualStudio.TestTools.UnitTesting;
using WiSoLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WiSoLibrary.Tests
{
	[TestClass()]
	public class SortQuestionTests
	{
		[TestMethod()]
		public void IsCorrectTest()
		{
			var doc = XDocument.Load(@"C:\Users\Mümmelmann\Documents\visual studio 2017\Projects\WiSoAbschluss\Klausuren\10_So_WISO\questions.xml");

			var aSortQuestion = doc.Root.Elements("Question").Single(p => p.Attribute("Number").Value == "5");
			var ques = parseSortQuestion(aSortQuestion);			
			
		}
		 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="question"></param>
		/// <returns></returns>
		private SortQuestion parseSortQuestion(XElement question)
		{
			//check if the question is of type Sort
			if (!question.Attribute("Type").Value.Equals("Sort", StringComparison.OrdinalIgnoreCase))
				throw new Exception("Wrong question type");
	
			//create a temporary list of answers
			List<MatchedAnswer> answers = new List<MatchedAnswer>();

			//Read out the possible answers and the correct answers
			var possibleAnswer = question.Element("Answers").Elements("Answer");
			var correctStep = question.Element("CorrectAnswers").Element("Answer").Attribute("Number").Value.Split(',');

			//check if the numbers match
			if (possibleAnswer.Count() != correctStep.Count())
				throw new Exception();

			//itterate thorugh the correct stepts 
			for(int i = 0; i < correctStep.Length; i++)
			{
				//get the correct step for this occasion
				var step = correctStep[i];
				var fitting = possibleAnswer.Single(p => p.Attribute("Number").Value == step);
				//add the step
				answers.Add(new MatchedAnswer(fitting.Value, i + 1));
			}

			//return the question
			return new SortQuestion(question.Element("Text").Value, answers.ToArray());
		}
	}
}