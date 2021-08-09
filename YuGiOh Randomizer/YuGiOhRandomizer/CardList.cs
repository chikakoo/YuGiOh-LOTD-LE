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
		public List<Card> MainDeckCards { get; set; }

		[JsonIgnore]
		public List<Card> ExtraDeckCards { get; set; }

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
			cardList.PopulateDeckCards();

			return cardList;
		}

		/// <summary>
		/// Updates the "All" list without cards we don't want to include
		/// Includes normally banned cards, temporarily banned cards, and all non-specific main deck monsters (for now)
		/// Assumes it's already populated
		/// </summary>
		private void FilterCards()
		{
			All = All.Where(x =>
				x.BanInfo != BanListTypes.Banned &&
				!x.TempBan &&
				x.Race != RaceTypes.Ritual &&
				x.Type != CardTypes.ToonMonster &&
				x.Type != CardTypes.RitualMonster &&
				x.Type != CardTypes.RitualEffectMonster
			).ToList();
		}

		/// <summary>
		/// Assumes that All is already populated
		/// Populates the main and extra deck cards using the cards in All
		/// </summary>
		private void PopulateDeckCards()
		{
			MainDeckCards = All.Where(x => x.DeckType == DeckTypes.Main).ToList();
			ExtraDeckCards = All.Where(x => x.DeckType == DeckTypes.Extra).ToList();
		}
	}
}
