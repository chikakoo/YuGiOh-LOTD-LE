using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace YuGiOhRandomizer
{
	public enum NameFilterType
	{
		/// <summary>
		/// (default)
		/// Wild cards - replaces all * with a regex for one or more characters
		/// So, *A* would contain all cards that have an "A" in them, anywhere
		/// A* would be all cards that start with an A (or just ARE A)
		/// </summary>
		[EnumMember(Value = "Wildcard")]
		Wildcard,

		/// <summary>
		/// Matches any cards where the string IS exactly one of the words in the name
		/// Words are seaprated by non A-Z characters
		/// </summary>
		[EnumMember(Value = "MatchWholeWord")]
		MatchWholeWord,

		/// <summary>
		/// Can be any regex
		/// </summary>
		[EnumMember(Value = "Regex")]
		Regex,

		/// <summary>
		/// A regex that will match against any one word in the card name
		/// Words are separated by non A-Z characters
		/// </summary>
		[EnumMember(Value = "RegexAnyWord")]
		RegexAnyWord
	}

	public class Filter
	{
		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public NameFilterType NameFilterType { get; set; }

		[JsonProperty]
		public List<GeneralCardTypes> GeneralCardTypes { get; set; }

		[JsonProperty]
		public List<CardTypes> CardTypes { get; set; }

		[JsonProperty]
		public Range LevelRange { get; set; }

		[JsonProperty]
		public bool? IsPendulum { get; set; }

		[JsonProperty]
		public bool? IsTuner { get; set; }

		[JsonProperty]
		public List<string> Archetypes { get; set; }

		[JsonProperty]
		public List<RaceTypes> Types { get; set; }

		[JsonProperty]
		public List<string> Attributes { get; set; }

		[JsonProperty]
		public Range AttackRange { get; set; }

		[JsonProperty]
		public Range DefenseRange { get; set; }

		/// <summary>
		/// Returns whether the given card passes the filter
		/// </summary>
		/// <param name="card">The card</param>
		/// <returns>True if so, false otherwise</returns>
		public bool DoesCardPassFilter(Card card)
		{
			return DoesCardPassNameFilter(card) &&
				(GeneralCardTypes == null || GeneralCardTypes.Contains(card.GeneralCardType)) &&
				(CardTypes == null || CardTypes.Contains(card.Type)) &&
				(LevelRange == null || LevelRange.IsInRange(card.Level)) &&
				(IsPendulum == null || card.IsPendulum == IsPendulum) &&
				(IsTuner == null || card.IsTuner == IsTuner) &&
				(Archetypes == null || Archetypes.Contains(card.Archetype)) &&
				(Types == null || Types.Contains(card.Race)) &&
				(Attributes == null || Attributes.Contains(card.Attribute)) &&
				(AttackRange == null || AttackRange.IsInRange(card.Attack)) &&
				(DefenseRange == null || DefenseRange.IsInRange(card.Defense));
		}

		/// <summary>
		/// Checks whether the card pases the name filter
		/// </summary>
		/// <param name="card">The card to check</param>
		/// <returns>True if so, false otherwise</returns>
		private bool DoesCardPassNameFilter(Card card)
		{
			string cardName = card.Name;
			if (string.IsNullOrWhiteSpace(cardName) || string.IsNullOrWhiteSpace(Name))
			{
				return true;
			}
			string patternName = Name.ToLower();
			string regex = patternName;

			switch (NameFilterType)
			{
				case NameFilterType.Wildcard:
					regex = GetPatternForWildcard(patternName);
					break;
				case NameFilterType.MatchWholeWord:
					regex = GetPatternForWholeWord(patternName);
					break;
				case NameFilterType.RegexAnyWord:
					regex = GetPatternForRegexAnyWord(patternName);
					break;
			}

			return Regex.IsMatch(cardName.ToLower(), regex);
		}

		private string GetPatternForWildcard(string name)
		{
			return $"^{Regex.Escape(name).Replace("\\*", ".*")}$";
		}

		private string GetPatternForWholeWord(string name)
		{
			return $"([^a-z]|^){Regex.Escape(name)}([^a-z]|$)";
		}

		private string GetPatternForRegex(string name)
		{
			return name;
		}

		private string GetPatternForRegexAnyWord(string name)
		{
			return $"([^a-z]|^){name}([^a-z]|$)";
		}

		/// <summary>
		/// For printing to the log
		/// </summary>
		/// <returns>The string to print</returns>
		public override string ToString()
		{
			return Name;
		}
	}
}
