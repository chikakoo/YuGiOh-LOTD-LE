using System;
using System.Collections.Generic;

namespace YuGiOh_Save_Deck_Extractor
{
	public static class Extensions
	{
		#region Public Fields

		public static Random rng = new Random();

		#endregion Public Fields

		#region Private Fields

		private static readonly int[] Empty = new int[0];

		#endregion Private Fields

		#region Public Methods

		public static int[] Locate(this byte[] self, byte[] candidate)
		{
			if (IsEmptyLocate(self, candidate))
				return Empty;

			var list = new List<int>();

			for (int i = 0; i < self.Length; i++)
			{
				if (!IsMatch(self, i, candidate))
					continue;

				list.Add(i);
			}

			return list.Count == 0 ? Empty : list.ToArray();
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		#endregion Public Methods

		#region Private Methods

		private static bool IsEmptyLocate(byte[] array, byte[] candidate)
		{
			return array == null
				|| candidate == null
				|| array.Length == 0
				|| candidate.Length == 0
				|| candidate.Length > array.Length;
		}

		private static bool IsMatch(byte[] array, int position, byte[] candidate)
		{
			if (candidate.Length > (array.Length - position))
				return false;

			for (int i = 0; i < candidate.Length; i++)
				if (array[position + i] != candidate[i])
					return false;

			return true;
		}

		#endregion Private Methods
	}
}
