using System;
using System.Collections.Generic;

namespace YuGiOhRandomizer
{
	public static class RandomHelper
	{
		public static bool GetNextBoolean()
		{
			return new Random().Next(0, 2) == 0;
		}

		public static bool GetNextBoolean(int percentage)
		{
			if (percentage < 0 || percentage > 100)
			{
				throw new ArgumentException("Percentage is not between 0 and 100!");
			}

			return new Random().Next(0, 100) < percentage;
		}

		public static T GetRandomValueFromList<T>(List<T> list)
		{
			if (list == null || list.Count == 0)
			{
				throw new ArgumentException("Attempted to get a random value out of an empty list!");
			}

			return list[new Random().Next(list.Count)];
		}

		public static T GetAndRemoveRandomValueFromList<T>(List<T> list)
		{
			if (list == null || list.Count == 0)
			{
				throw new ArgumentException("Attempted to get a random value out of an empty list!");
			}
			int selectedIndex = new Random().Next(list.Count);
			T selectedValue = list[selectedIndex];
			list.RemoveAt(selectedIndex);
			return selectedValue;
		}

		public static List<T> GetRandomValuesFromList<T>(List<T> inputList, int numberOfvalues)
		{
			List<T> listToChooseFrom = new List<T>(inputList); // Don't modify the original list
			List<T> randomValues = new List<T>();
			if (listToChooseFrom == null || listToChooseFrom.Count == 0)
			{
				throw new ArgumentException("Attempted to get random values out of an empty list!");
			}

			int numberOfIterations = Math.Min(numberOfvalues, listToChooseFrom.Count);
			for (int i = 0; i < numberOfIterations; i++)
			{
				randomValues.Add(GetAndRemoveRandomValueFromList(listToChooseFrom));
			}

			return randomValues;
		}
	}
}
