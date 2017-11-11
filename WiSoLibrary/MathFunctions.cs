using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	public static class MathFunctions
	{
		/// <summary>
		/// Uses first int.Parse() than Regex.Replace(\D+ , "") --> Formats all nonumbers away bevor parsig+
		/// 
		/// If any other eror occurs, it returns 0 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int StringToInt(string s)
		{
			try
			{
				return int.Parse(s);
			}
			catch (FormatException)
			{
				s = Regex.Replace(s, "\\D+", "");
				return int.Parse((s == "")? "0": s);
			}
			catch (Exception)
			{
				return 0;
			}
		}



		/// <summary>
		/// Converts a ConsoleKey character to it's representing interger value --> D0 ~> 0, D1 ~> 1 ect.
		/// 
		/// If the passed argument isn't a number it will return -1
		/// </summary>
		/// <returns></returns>
		public static int ConsoleKeyToInt(ConsoleKeyInfo cki)
		{
			return ConsoleKeyToInt(cki.Key);
		}

		/// <summary>
		/// Converts a ConsoleKey character to it's representing interger value --> D0 ~> 0, D1 ~> 1 ect.
		/// 
		/// If the passed argument isn't a number it will return -1
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int ConsoleKeyToInt(ConsoleKey key)
		{
			switch (key)
			{
				case ConsoleKey.NumPad0:
				case ConsoleKey.D0: return 0;
				case ConsoleKey.NumPad1:
				case ConsoleKey.D1: return 1;
				case ConsoleKey.NumPad2:
				case ConsoleKey.D2: return 2;
				case ConsoleKey.NumPad3:
				case ConsoleKey.D3: return 3;
				case ConsoleKey.NumPad4:
				case ConsoleKey.D4: return 4;
				case ConsoleKey.NumPad5:
				case ConsoleKey.D5: return 5;
				case ConsoleKey.NumPad6:
				case ConsoleKey.D6: return 6;
				case ConsoleKey.NumPad7:
				case ConsoleKey.D7: return 7;
				case ConsoleKey.NumPad8:
				case ConsoleKey.D8: return 8;
				case ConsoleKey.NumPad9:
				case ConsoleKey.D9: return 9;
				default: return -1;
			}
		}

		/// <summary>
		/// Yield returns the array converted to int
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static IEnumerable<int> ConvertToIntArray(IEnumerable<string> array)
		{
			foreach (var s in array)		
				yield return StringToInt(s);			
		}

	}
}
