using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// Gets its data from deckDistributionSettings.json
	/// 
	/// A class that represents how decks will be created
	/// It consists of an array of tasks to complete each time a random deck is created
	/// If the main deck ends up contianing 60 cards (or 15 for extra decks), it will stop immediately
	/// If it ends with less than 40, then it will randomly pick cards from there
	/// </summary>
	public class DeckDistributionSettings
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

		/// <summary>
		/// The list of tasks to execute
		/// </summary>
		[JsonProperty("DeckDistributionSettings")]
		public List<DeckDistributionTask> Tasks { get; set; }

		/// <summary>
		/// Get an instance of the settings object
		/// </summary>
		/// <returns />
		public static DeckDistributionSettings GetSettingsInstance()
		{
			string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			DeckDistributionSettings settings = JsonConvert.DeserializeObject<DeckDistributionSettings>(
				File.ReadAllText($@"{directory}\deckDistributionSettings.json")
			);
			return settings;
		}
	}
}
