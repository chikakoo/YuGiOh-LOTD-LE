using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// Gets its data from deckDistributionSettings.json
	/// A class that represents how decks will be created
	/// </summary>
	public class DeckDistributionSettings
	{
		[JsonProperty]
		public string PlayerDeck { get; set; }

		[JsonProperty]
		public string OpponentDeck { get; set; }

		/// <summary>
		/// A dictionary of deck keys to their settings - that is, a map of keys to
		/// various ways decks can be generated
		/// </summary>
		[JsonProperty]
		public Dictionary<string, DeckDistributionSetting> DeckDistributionSettingsMap { get; set; }

		/// <summary>
		/// The list of tasks to execute
		/// </summary>
		[JsonProperty("DeckDistributionSettings")]
		public List<DeckCreationTask> Tasks { get; set; }

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
