using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiSoLibrary
{
	public static class ListRandomizer
	{
		/// <summary>
		/// Randomizes the passed List
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static List<T> RandomizeList<T>(this List<T> list)
		{
			//initilitze the randomness
			Random random = new Random();
			//creat the temporaty list
			List<T> tmpList = new List<T>(list);
			//create the new öst
			List<T> newList = new List<T>(list.Count);

			//while tmplis thas items
			while(tmpList.Count > 0)
			{
				//remove item from templist and ad it to newlist
				var item = tmpList[random.Next(tmpList.Count)];
				tmpList.Remove(item);
				newList.Add(item);
			}
			

			return newList;
		}

		/// <summary>
		/// Randomizes an array heavily (calls toList, RandomizeList, ToArray)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static T[] RandomizeArray<T>(this T[] array)
		{
			return array.ToList().RandomizeList().ToArray();
		}
	}
}
