using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WiSoLibrary;

namespace MultipleChoiceConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var d = Exam.GetExamFromXML(@"C:\Users\Mümmelmann\Google Drive\Alle WISO\Korrigierte\16_So_WISO\questions.xml");
			int points = 0, totalPoints = 0;
			List<StudentAnswer> givenAnswers = new List<StudentAnswer>();
			int counter = 1;

			//itterate through all questions
			foreach (var question in d.Questions.Randomize())
			{
				//Spaghetti implemented counter
				Console.WriteLine($"Question {counter++}/{d.Questions.Count()}");
				
				//display the questions
				Console.WriteLine(question.Text);
				Console.WriteLine();

				//randomize the answer and siplay them
				var answers = question.Answers.Randomize();
				for (int i = 0; i < answers.Length; i++)				
					Console.WriteLine($"\t{i + 1}. {answers[i].Text}");

				//add the correct answers
				totalPoints += question.CorrectAnswers.Count();

				Console.WriteLine();
				Console.WriteLine();

				//instantiate a new list wich contains the indizies of ansers wich the user has given
				List<Answer> selectedAnswer = new List<Answer>(question.CorrectAnswers.Count());

				//check if moa than 1 answer
				if(question.CorrectAnswers.Count() > 1)
				{

					#region [ Read Input ]

					//get the input
					IEnumerable<int> input;

					//while not enough answers
					do
					{
						Console.WriteLine($"Type in exactly {question.CorrectAnswers.Count()} numbers (space or comma seperated), that you think are correct and confirm with an enter.");
						input = MathFunctions.ConvertToIntArray(Regex.Split(Console.ReadLine().Trim(), "[, ]").Distinct());
						Console.WriteLine();
					//do while the lenghts arn't the same or any number is smaller than 1 or greater than the length
					} while (input.Count() != question.CorrectAnswers.Count() || input.Any(i => i < 1 || i > answers.Length));

					#endregion [ Read Input ]

					//add each index bevor the typed one
					foreach (int i in input)					
						selectedAnswer.Add(answers[i - 1]);					
				}
				else
				{
					#region [ Read Input ]
					int input;
					do
					{
						Console.WriteLine("Type in the number of the correct answer.");
						input = MathFunctions.ConsoleKeyToInt(Console.ReadKey());
						Console.WriteLine();
						Console.WriteLine();
					} while (input < 1 || input > answers.Length);

					#endregion	

					selectedAnswer.Add(answers[input - 1]);
				}

				#region [ add points ]

				//check if all answers are correct
				if (selectedAnswer.All(p => p.IsCorrect))
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Correct!");
					Console.ForegroundColor = ConsoleColor.Gray;

					//add the number of correct ansers
					points += selectedAnswer.Count;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Wrong!");
					Console.ForegroundColor = ConsoleColor.Gray;

					points += selectedAnswer.Where(p => p.IsCorrect).Count();
					givenAnswers.Add(new StudentAnswer{ Question = question, GivenAnswers = selectedAnswer.ToArray()});
				}

				#endregion

				System.Threading.Thread.Sleep(500);

				Console.Clear();
			}

			//display conclusion
			Console.WriteLine($"You got {points}/{totalPoints} correct.");
			Console.WriteLine();
			Console.WriteLine("The fawlty quesions are...");
			Console.WriteLine();
			foreach (var item in givenAnswers)
			{
				//Display quesions
				Console.WriteLine("Quesion:");
				Console.WriteLine(item.Question.Text.Trim());
				Console.WriteLine();

				//display correct answers
				Console.WriteLine($"The correct answer{(item.Question.CorrectAnswers.Count() > 1? "s are": " is")}:");
				Console.ForegroundColor = ConsoleColor.Green;
				foreach (var s in item.Question.CorrectAnswers)
					Console.WriteLine(s.Text.Trim());
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Gray;

				//display his answers
				Console.WriteLine($"Your wrong answer{(item.GivenAnswers.Count() > 1 ? "s where" : " was")}:");
				Console.ForegroundColor = ConsoleColor.Red;
				foreach (var s in item.GivenAnswers.Where(p => !p.IsCorrect))
					Console.WriteLine(s.Text.Trim());
				Console.ForegroundColor = ConsoleColor.Gray;

				Console.WriteLine();
				Console.WriteLine();
			}


			Console.WriteLine("Press any key to exit!");
			Console.ReadKey();
			Environment.Exit(0);
		}
	}
}
