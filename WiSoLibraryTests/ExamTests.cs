using Microsoft.VisualStudio.TestTools.UnitTesting;
using WiSoLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary.Tests
{
	[TestClass()]
	public class ExamTests
	{
		[TestMethod()]
		public void GetExamFromXMLTest()
		{
			Exam ex1 = Exam.GetExamFromXML(@"D:\Git Projects\MultipleChoice\WISO_W2016_A_PDF24\questions.xml");
			Assert.Fail();
		}
	}
}