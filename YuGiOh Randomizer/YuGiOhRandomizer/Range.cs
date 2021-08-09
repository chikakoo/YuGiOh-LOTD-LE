using Newtonsoft.Json;
using System;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// Represents a range of values - used for possible ranges of randomly generated values
	/// </summary>
	public class Range
	{
		[JsonProperty]
		public int Min { get; set; }

		[JsonProperty]
		public int Max { get; set; }

		/// <summary>
		/// Constructor - has safety checks for whether the min and max values are correct
		/// </summary>
		/// <param name="minValue">The minimum value in the range</param>
		/// <param name="maxValue">The maximum value in the range</param>
		[JsonConstructor]
		public Range(int min, int max)
		{
			if (min < max)
			{
				Min = min;
				Max = max;
			}

			else
			{
				Min = max;
				Max = min;
			}
		}

		/// <summary>
		/// Gets a random value between the min and max value, inclusive
		/// </summary>
		/// <returns />
		public int GetRandomValue()
		{
			return new Random().Next(Min, Max + 1);
		}

		/// <summary>
		/// Gets a random value between the min and max value, inclusive
		/// </summary>
		/// <param name="min">The min value</param>
		/// <param name="maxValue">The max value</param>
		/// <returns />
		public static int GetRandomValue(int min, int max)
		{
			return new Range(min, max).GetRandomValue();
		}

		/// <summary>
		/// Returns whether the givne value is within the range
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <returns></returns>
		public bool IsInRange(int value)
		{
			return value >= Min && value <= Max;
		}

		/// <summary>
		/// Used for debugging
		/// </summary>
		/// <returns />
		public override string ToString()
		{
			return $"{Min}-{Max}";
		}
	}
}
