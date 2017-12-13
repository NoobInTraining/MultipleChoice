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
		string Text { get; }

		bool IsCorrect(params object[] param);
	}
}
