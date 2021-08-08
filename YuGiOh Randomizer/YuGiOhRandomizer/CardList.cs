using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YuGiOhRandomizer
{
	public class CardList
	{
		[JsonProperty("ExtractedData")]
		public List<Card> All { get; set; }

		[JsonIgnore]
		public List<Card> NonSpecificMonsters // Others include Ritual, Extra Deck, and Toons
		{
			get
			{
				return All.Where(x => x.IsNonSpecificMonster).ToList();
			}
		}

		[JsonIgnore]
		public List<Card> ExtraDeckMonsters { get; } //TODO later - not testing this right now

		[JsonIgnore]
		public List<Card> Spells
		{
			get
			{
				return All.Where(x => x.IsSpellCard).ToList();
			}
		}

		[JsonIgnore]
		public List<Card> Traps
		{
			get
			{
				return All.Where(x => x.IsTrapCard).ToList();
			}
		}

		/// <summary>
		/// Get an instance of all the card data
		/// </summary>
		/// <returns />
		public static CardList GetCardListInstance()
		{
			string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			CardList cardList = JsonConvert.DeserializeObject<CardList>(
				File.ReadAllText($@"{directory}\cardData.json")
			);
			cardList.FilterCards();

			return cardList;
		}

		/// <summary>
		/// Updates the "All" list without cards we don't want to include
		/// Assumes it's already populated
		/// </summary>
		private void FilterCards()
		{
			All = All.Where(x =>
				!x.TempBan && // Filter temporarily banned cards
				x.Race != RaceTypes.Ritual // Filter ritual spell cards
			).ToList();
		}
	}
}
