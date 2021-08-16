using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace YuGiOhRandomizer
{
	public class FilterSet
	{
		/// <summary>
		/// The name pattern enum
		/// </summary>
		public enum FilterSetType
		{
			/// <summary>
			/// Goes through each value in the list one at a time, cycling around to the front when the end is reached
			/// Will exit out if every possible value is exhausted
			/// </summary>
			[EnumMember(Value = "Round Robin")]
			RoundRobin,

			/// <summary>
			/// Goes through each list one at a time; when one fails to add anything, THEN it will move onto the next value
			/// </summary>
			[EnumMember(Value = "Fallback")]
			Fallback
		}

		/// <summary>
		/// The type (see the NamePatternType enum)
		/// </summary>
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public FilterSetType Type { get; set; }

		/// <summary>
		/// A list of filters
		/// </summary>
		[JsonProperty]
		public List<Filter> Filters { get; set; } = new List<Filter>();

		/// <summary>
		/// Whether we should shuffle the patterns when starting or restarting the loop through them
		/// </summary>
		[JsonProperty]
		public bool ShuffleFilters { get; set; }

		/// <summary>
		/// The current index in the pattern list
		/// </summary>
		[JsonIgnore]
		private int CurrentIndex { get; set; }

		/// <summary>
		/// The current pattern, computed from Patterns and CurrentIndex
		/// </summary>
		[JsonIgnore]
		public Filter CurrentFilter
		{
			get
			{
				if (!Filters.Any())
				{
					return null;
				}
				return Filters[CurrentIndex];
			}
		}

		/// <summary>
		/// Whether we're done
		/// - If the patterns are empty, there's nothing left to do
		/// - If the index is out of range, it means we've reached the end in the fallback case
		/// </summary>
		[JsonIgnore]
		public bool Completed
		{
			get
			{
				return !Filters.Any() || CurrentIndex >= Filters.Count;
			}
		}

		/// <summary>
		/// Constructor - shuffles the patterns if required
		/// </summary>
		/// <param name="shuffleFilters">Whether the shuffle patterns</param>
		/// <param name="filters">The filters</param>
		[JsonConstructor]
		public FilterSet(bool shuffleFilters, List<Filter> filters)
		{
			ShuffleFilters = shuffleFilters;
			Filters = filters;
			TryShufflePatterns();
		}

		/// <summary>
		/// Shuffles the patterns if the setting is on
		/// </summary>
		public void TryShufflePatterns()
		{
			if (ShuffleFilters)
			{
				Filters.Shuffle();
			}
		}

		/// <summary>
		/// Filters the list of cards down based on the current pattern
		/// </summary>
		/// <param name="cardList">The list of cards</param>
		/// <returns>The filtered list</returns>
		public List<Card> Filter(List<Card> cardList)
		{
			if (CurrentFilter == null)
			{
				return cardList;
			}

			return cardList.Where(x => CurrentFilter.DoesCardPassFilter(x)).ToList();
		}

		/// <summary>
		/// Round Robin:
		/// Records that the current pattern has failed by removing it from the list
		/// If the current index was at the very end, then set it back to the start, since it's
		/// meant to loop!
		/// 
		/// Fallback:
		/// Advances the index by 1 to try the next pattern
		/// </summary
		public void OnCardAddFailure()
		{
			if (Type == FilterSetType.RoundRobin)
			{
				Filters.RemoveAt(CurrentIndex);
				if (CurrentIndex >= Filters.Count)
				{
					CurrentIndex = 0;
					TryShufflePatterns(); // We're starting over, so we should shuffle
				}
			}

			else
			{
				AdvanceIndex();
			}
		}

		/// <summary>
		/// If a card was added successfully...
		/// - Round Robin should advance to the next value
		/// - Fallback shouldn't do anything, since we're still good to go
		/// </summary>
		public void OnCardAdded()
		{
			if (Type == FilterSetType.RoundRobin)
			{
				AdvanceIndex();
			}
		}

		/// <summary>
		/// Advances the list's index
		/// </summary>
		private void AdvanceIndex()
		{
			CurrentIndex++;
			if (CurrentIndex >= Filters.Count)
			{
				// If fallback, DO NOT move back to 0 so that Completed can be computed - we're done in that case!
				if (Type == FilterSetType.Fallback)
				{
					return;
				}

				// If round robin, we just loop back around to the start again
				CurrentIndex = 0;
				TryShufflePatterns(); // We're starting over, so we should shuffle
			}
		}
	}
}
