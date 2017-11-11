using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiSoLibrary;

namespace MultipleChoiceConsole
{
	struct StudentAnswer
	{
		public Question Question { get; set; }

		public Answer[] GivenAnswers{ get; set; }
	}
}
