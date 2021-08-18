using Newtonsoft.Json;
using System.Collections.Generic;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// Gets its data from deckDistributionSettings.json
	/// 
	/// A class that represents how a deck will be created.
	/// 
	/// It consists of an array of tasks to complete each time a random deck is created
	/// If the main deck ends up contianing 60 cards (or 15 for extra decks), it will stop immediately
	/// If it ends with less than 40, then it will randomly pick cards from there
	/// </summary>
	public class DeckDistributionSetting
	{
		[JsonProperty]
		public Range MainDeckSize { get; set; }

		[JsonProperty]
		public Range ExtraDeckSize { get; set; }

		[JsonProperty]
		public bool MainDeckAddRandomCardsIfNeeded { get; set; }

		[JsonProperty]
		public bool ExtraDeckAddRandomCardsIfNeeded { get; set; }

		[JsonProperty]
		public bool IgnoreBanList { get; set; }

		[JsonProperty]
		public List<DeckCreationTask> DeckCreationTasks { get; set; }
	}
}
