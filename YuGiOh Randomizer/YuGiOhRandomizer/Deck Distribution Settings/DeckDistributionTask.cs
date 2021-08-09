using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// Represents one task for deck creation
	/// For example, addding 5 monster cards that are level 4
	/// </summary>
	public class DeckDistributionTask
	{
		/// <summary>
		/// The general type of card
		/// </summary>
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public GeneralCardTypes GeneralCardType { get; set; }

		/// <summary>
		/// A list of general types to NOT include - this is used with the Random types
		/// </summary>
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public List<GeneralCardTypes> ExcludedGeneralTypes { get; set; } = new List<GeneralCardTypes>();

		/// <summary>
		/// The level range to grab for monsters
		/// ONLY useful for monsters
		/// </summary>
		[JsonProperty("levelRange")]
		public Range MonsterLevelRange { get; set; }


		/// <summary>
		/// The number of cards to pick
		/// </summary>
		[JsonProperty]
		public Range CardRange { get; set; }

		/// <summary>
		/// The type of deck this task is for
		/// </summary>
		[JsonIgnore]
		public DeckTypes DeckType
		{
			get
			{
				switch (GeneralCardType)
				{
					case GeneralCardTypes.Monster:
					case GeneralCardTypes.Spell:
					case GeneralCardTypes.Trap:
					case GeneralCardTypes.RandomMain:
						return DeckTypes.Main;

					case GeneralCardTypes.Fusion:
					case GeneralCardTypes.Synchro:
					case GeneralCardTypes.Xyz:
					case GeneralCardTypes.Link:
					case GeneralCardTypes.RandomExtra:
						return DeckTypes.Extra;

					default:
						throw new ArgumentException("Tried to get deck type for an unknown card type somehow!");
				}
			}
		}

		/// <summary>
		/// Gets a task for random main deck cards
		/// </summary>
		/// <returns />
		public static DeckDistributionTask GetTaskForRandomMainCards(int numberOfCards)
		{
			return new DeckDistributionTask()
			{
				GeneralCardType = GeneralCardTypes.RandomMain,
				CardRange = new Range(numberOfCards, numberOfCards)
			};
		}

		/// <summary>
		/// Gets a task for random extra deck cards
		/// </summary>
		/// <returns />
		public static DeckDistributionTask GetTaskForRandomExtraCards(int numberOfCards)
		{
			return new DeckDistributionTask()
			{
				GeneralCardType = GeneralCardTypes.RandomExtra,
				CardRange = new Range(numberOfCards, numberOfCards)
			};
		}

		/// <summary>
		/// Just used for debugging purposes
		/// </summary>
		/// <returns />
		public override string ToString()
		{
			string cardType = Enum.GetName(typeof(GeneralCardTypes), GeneralCardType);
			return $"{cardType}: Levels {MonsterLevelRange}; Cards: {CardRange}";
		}
	}
}
