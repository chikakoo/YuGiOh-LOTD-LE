using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YuGiOhRandomizer
{
	public class Filter
	{
		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public bool MatchWholeWord { get; set; }

		[JsonProperty]
		public List<GeneralCardTypes> GeneralCardTypes { get; set; }

		[JsonProperty]
		public Range LevelRange { get; set; }

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
				(LevelRange == null || LevelRange.IsInRange(card.Level)) &&
				(Archetypes == null || Archetypes.Contains(card.Archetype)) &&
				(Types == null || Types.Contains(card.Race)) &&
				(Attributes == null || Attributes.Contains(card.Attribute)) &&
				(AttackRange == null || AttackRange.IsInRange(card.Attack)) &&
				(DefenseRange == null || AttackRange.IsInRange(card.Defense));
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

			string regex = MatchWholeWord
				? $"([^A-Za-z]|^){Name.ToLower()}([^A-Za-z]|$)"
				: $"^{Regex.Escape(Name.ToLower()).Replace("\\*", ".*")}$";

			return Regex.IsMatch(cardName.ToLower(), regex);
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
