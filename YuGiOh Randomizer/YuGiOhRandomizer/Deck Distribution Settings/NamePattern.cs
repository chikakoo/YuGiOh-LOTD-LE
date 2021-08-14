using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace YuGiOhRandomizer
{
	public class NamePattern
	{
		/// <summary>
		/// The name pattern enum
		/// </summary>
		public enum NamePatternType
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
		public NamePatternType Type { get; set; }

		/// <summary>
		/// The patterns - * are wildcards. e.g. "A*" will get all cards starting with "A"
		/// </summary>
		[JsonProperty]
		public List<string> Patterns { get; set; } = new List<string>();

		/// <summary>
		/// Whether we should match the whole word in the card name
		/// e.g. "Pot" matches "Pot of Greed", but not "Potion"
		/// </summary>
		[JsonProperty]
		public bool MatchWholeWord { get; set; }

		/// <summary>
		/// The current index in the pattern list
		/// </summary>
		[JsonIgnore]
		private int CurrentIndex { get; set; }

		/// <summary>
		/// The current pattern, computed from Patterns and CurrentIndex
		/// </summary>
		[JsonIgnore]
		public string CurrentPattern
		{
			get
			{
				if (!Patterns.Any())
				{
					return null;
				}
				return Patterns[CurrentIndex];
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
				return !Patterns.Any() || CurrentIndex >= Patterns.Count;
			}
		}

		/// <summary>
		/// Filters the list of cards down based on the current pattern
		/// </summary>
		/// <param name="cardList">The list of cards</param>
		/// <returns>The filtered list</returns>
		public List<Card> Filter(List<Card> cardList)
		{
			if (CurrentPattern == null)
			{
				return cardList;
			}

			return cardList.Where(x => DoesCardNameMatch(x.Name)).ToList();
		}

		/// <summary>
		/// Whether this task matches the given card name
		/// </summary>
		/// <param name="name">The name</param>
		private bool DoesCardNameMatch(string name)
		{
			if (CurrentPattern == null)
			{
				return true; // In this case, there are no patterns!
			}

			string regex = MatchWholeWord
				? $"([^A-Za-z]|^){CurrentPattern.ToLower()}([^A-Za-z]|$)"
				: $"^{Regex.Escape(CurrentPattern.ToLower()).Replace("\\*", ".*")}$";

			return Regex.IsMatch(name.ToLower(), regex);
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
			if (Type == NamePatternType.RoundRobin)
			{
				Patterns.RemoveAt(CurrentIndex);
				if (CurrentIndex >= Patterns.Count)
				{
					CurrentIndex = 0;
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
			if (Type == NamePatternType.RoundRobin)
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
			if (CurrentIndex >= Patterns.Count)
			{
				// If fallback, DO NOT move back to 0 so that Completed can be computed - we're done in that case!
				if (Type == NamePatternType.Fallback)
				{
					return;
				}

				// If round robin, we just loop back around to the start again
				CurrentIndex = 0;
			}
		}
	}
}
