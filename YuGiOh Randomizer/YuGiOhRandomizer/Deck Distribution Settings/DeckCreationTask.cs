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
	public class DeckCreationTask
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
		/// Whether we allow level 0 cards if they don't fall into the level range requiremebts
		/// If this is true, then all Link monsters, for example, will be fair game even if there is a level range requirement
		/// </summary>
		[JsonProperty("allowLevel0IfNotInRange")]
		public bool AllowLevel0IfNotInRange { get; set; }

		/// <summary>
		/// The number of cards to pick
		/// </summary>
		[JsonProperty]
		public Range CardRange { get; set; }

		/// <summary>
		/// The pattern that the card name must match
		/// </summary>
		[JsonProperty]
		public FilterSet FilterSet { get; set; }

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
		/// Whether there's no more to do with the pattern
		/// </summary>
		[JsonIgnore]
		public bool IsPatternExhausted
		{
			get
			{
				if (FilterSet == null)
				{
					return false; // There is no pattern!
				}

				return FilterSet.Completed;
			}
		}

		/// <summary>
		/// Set if there's no name pattern and the add card failed
		/// </summary>
		[JsonIgnore]
		public bool ShouldExitNow { get; set; }

		/// <summary>
		/// Filter the card list based on the name pattern
		/// </summary>
		/// <param name="cardList"></param>
		/// <returns></returns>
		public List<Card> FilterNames(List<Card> cardList)
		{
			if (FilterSet == null)
			{
				return cardList;
			}

			return FilterSet.Filter(cardList);
		}

		/// <summary>
		/// Calls when an attempt to add a card is made and adjusts things accordingly
		/// </summary>
		/// <param name="result">The result of the attempt</param>
		public void OnAddCardAttempt(bool result)
		{
			if (result)
			{
				OnCardAdded();
			}

			else
			{
				OnCardAddFailure();
			}
		}

		/// <summary>
		/// Should be called when a card is added successfully
		/// </summary>
		private void OnCardAdded()
		{
			if (FilterSet != null)
			{
				FilterSet.OnCardAdded();
			}

		}

		/// <summary>
		/// Should be called when a card is NOT added successfully
		/// Will set the exit flag if we don't have a name pattern
		/// </summary>
		private void OnCardAddFailure()
		{
			if (FilterSet != null)
			{
				FilterSet.OnCardAddFailure();
			}

			else
			{
				ShouldExitNow = true;
			}
		}

		/// <summary>
		/// Gets a task for random main deck cards
		/// </summary>
		/// <returns />
		public static DeckCreationTask GetTaskForRandomMainCards(int numberOfCards)
		{
			return new DeckCreationTask()
			{
				GeneralCardType = GeneralCardTypes.RandomMain,
				CardRange = new Range(numberOfCards, numberOfCards)
			};
		}

		/// <summary>
		/// Gets a task for random extra deck cards
		/// </summary>
		/// <returns />
		public static DeckCreationTask GetTaskForRandomExtraCards(int numberOfCards)
		{
			return new DeckCreationTask()
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
