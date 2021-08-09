using System;
using System.Collections.Generic;
using System.Linq;

namespace YuGiOhRandomizer
{
	public class RandomDeck
	{
		private const int MinMainDeckSize = 40;
		private const int MaxMainDeckSize = 60;
		private const int MinExtraDeckSize = 0;
		private const int MaxExtraDeckSize = 15;

		/// <summary>
		/// Used to enforce the card limit
		/// </summary>
		private Dictionary<string, int> CardCount { get; set; } = new Dictionary<string, int>();

		/// <summary>
		/// The cards in the deck
		/// </summary>
		public List<Card> MainDeckCards { get; set; } = new List<Card>();

		/// <summary>
		/// The cards in the extra deck
		/// </summary>
		public List<Card> ExtraDeckCards { get; set; } = new List<Card>();

		/// <summary>
		/// The main deck size - taken from the settings and clamped to the min/max values
		/// </summary>
		public int MainDeckSize { get; set; }

		/// <summary>
		/// The extra deck size - taken from the settings and clamped to the min/max values
		/// </summary>
		public int ExtraDeckSize { get; set; }

		/// <summary>
		/// The number of monsters in the main deck
		/// </summary>
		public int MainDeckMonsterCount
		{
			get
			{
				return MainDeckCards.Count(x => x.GeneralCardType == GeneralCardTypes.Monster);
			}
		}

		/// <summary>
		/// The number of spells in the main deck
		/// </summary>
		public int MainDeckSpellCount
		{
			get
			{
				return MainDeckCards.Count(x => x.GeneralCardType == GeneralCardTypes.Spell);
			}
		}

		/// <summary>
		/// The number of traps in the main deck
		/// </summary>
		public int MainDeckTrapCount
		{
			get
			{
				return MainDeckCards.Count(x => x.GeneralCardType == GeneralCardTypes.Trap);
			}
		}

		/// <summary>
		/// Constructor - generates the random deck based on the settings
		/// </summary>
		public RandomDeck()
		{
			Generate();
			Validate();
		}

		/// <summary>
		/// Validates the deck sizes, etc. to ensure things don't break
		/// </summary>
		public void Validate()
		{
			if (MainDeckSize != MainDeckCards.Count)
			{
				throw new Exception("Main deck not the expected size!");
			}

			if (ExtraDeckSize != ExtraDeckCards.Count)
			{
				throw new Exception("Main deck not the expected size!");
			}

			if (MainDeckCards.Any(x => x.DeckType != DeckTypes.Main))
			{
				throw new Exception("Extra deck card found in main deck!");
			}

			if (ExtraDeckCards.Any(x => x.DeckType != DeckTypes.Extra))
			{
				throw new Exception("Main deck card found in extra deck!");
			}
		}

		/// <summary>
		/// Adds all the cards to the decks
		/// Includes sorting each deck by name
		/// </summary>
		private void Generate()
		{
			SetDeckSizes();
			AddCardsFromTasks();
			AddAddtionalCards();
			MainDeckCards = MainDeckCards.OrderBy(x => x.Name).ToList();
			ExtraDeckCards = ExtraDeckCards.OrderBy(x => x.Name).ToList();
		}

		/// <summary>
		/// Sets up the deck sizes based on the settings and the hard limits
		/// </summary>
		private void SetDeckSizes()
		{
			int mainDeckSetingsSize = Program.DeckDistributionSettings.MainDeckSize.GetRandomValue();
			MainDeckSize = Math.Clamp(mainDeckSetingsSize, MinMainDeckSize, MaxMainDeckSize);

			int extraDeckSettingsSize = Program.DeckDistributionSettings.ExtraDeckSize.GetRandomValue();
			ExtraDeckSize = Math.Clamp(extraDeckSettingsSize, MinExtraDeckSize, MaxExtraDeckSize);
		}

		/// <summary>
		/// Goes through and adds all the cards from the set tasks
		/// If it reaches the deck size limit, it will stop where it left off
		/// </summary>
		public void AddCardsFromTasks()
		{
			foreach (DeckDistributionTask task in Program.DeckDistributionSettings.Tasks)
			{
				AddCardsFromTask(task);
			}
		}

		/// <summary>
		/// Adds additional cards to the decks
		/// This is done if the deck size is not yet met
		/// </summary>
		public void AddAddtionalCards()
		{
			int mainDeckCardsRemaining = MainDeckSize - MainDeckCards.Count;
			if (mainDeckCardsRemaining > 0)
			{
				AddCardsFromTask(DeckDistributionTask.GetTaskForRandomMainCards(mainDeckCardsRemaining));
			}

			int extraDeckCardsRemaining = ExtraDeckSize - ExtraDeckCards.Count;
			if (extraDeckCardsRemaining > 0)
			{
				AddCardsFromTask(DeckDistributionTask.GetTaskForRandomExtraCards(extraDeckCardsRemaining));
			}
		}

		/// <summary>
		/// Adds the cards from the given task
		/// </summary>
		/// <param name="task">The task</param>
		private void AddCardsFromTask(DeckDistributionTask task)
		{
			List<Card> cardsToChooseFrom = GetCardsToChooseFrom(task);

			int cardsToAdd = task.CardRange.GetRandomValue();
			if (cardsToChooseFrom.Count < cardsToAdd)
			{
				throw new Exception("Could potentially run out of cards to add! Did you restrict your card list too much?");
			}

			List<Card> deck = task.DeckType == DeckTypes.Main ? MainDeckCards : ExtraDeckCards;
			int maxSize = task.DeckType == DeckTypes.Main ? MainDeckSize : ExtraDeckSize;
			for (int i = 0; i < cardsToAdd && deck.Count < maxSize; i++)
			{
				AddCard(cardsToChooseFrom, deck);
			}
		}

		/// <summary>
		/// Get the filtered list of all cards to randomly choose from
		/// </summary>
		/// <param name="task">The task to base the filters off of</param>
		/// <returns>The card pool to randomly choose from</returns>
		private List<Card> GetCardsToChooseFrom(DeckDistributionTask task)
		{
			List<Card> cardsToChooseFrom = task.DeckType == DeckTypes.Main
				? Program.CardList.MainDeckCards
				: Program.CardList.ExtraDeckCards;

			// If we're picking a random card, no need to filter anything else!
			if (task.GeneralCardType == GeneralCardTypes.RandomMain || task.GeneralCardType == GeneralCardTypes.RandomExtra)
			{
				return GetCardsToChooseFromForRandomType(task, cardsToChooseFrom);
			}

			cardsToChooseFrom = cardsToChooseFrom.Where(x =>
				x.GeneralCardType == task.GeneralCardType &&
				(
					x.Level == 0 || task.MonsterLevelRange.Max == 0 || // Ignore all cards with a level of 0 or a limit of 0
					task.MonsterLevelRange.IsInRange(x.Level)
				)
			).ToList();

			return cardsToChooseFrom;
		}

		/// <summary>
		/// Gets the list of cards to choose from for one of the random GeneralCardTypes
		/// </summary>
		/// <param name="task">The current task</param>
		/// <param name="baseList">The list to start with</param>
		/// <returns>The filtered list - currently only excludes types, assuming that's populated</returns>
		private List<Card> GetCardsToChooseFromForRandomType(DeckDistributionTask task, List<Card> baseList)
		{
			return baseList.Where(x => !task.ExcludedGeneralTypes.Contains(x.GeneralCardType)).ToList();
		}

		/// <summary>
		/// Adds a card from the given list
		/// Will respect the ban values
		/// </summary>
		/// <param name="listToChooseFrom">The list of cards to randomly choose from</param>
		/// <param name="deckToAddTo">The deck to add to</param>
		private void AddCard(List<Card> listToChooseFrom, List<Card> deckToAddTo)
		{
			Card cardToAdd = null;
			do
			{
				cardToAdd = RandomHelper.GetRandomValueFromList(listToChooseFrom);
			} while (!DoesCardPassBanCheck(cardToAdd));

			if (CardCount.ContainsKey(cardToAdd.Name))
			{
				CardCount[cardToAdd.Name]++;
			}

			else
			{
				CardCount[cardToAdd.Name] = 1;
			}

			deckToAddTo.Add(cardToAdd);
		}

		/// <summary>
		/// Checks whether there are already too many of the card we're trying to add
		/// </summary>
		/// <param name="card"></param>
		/// <returns />
		private bool DoesCardPassBanCheck(Card card)
		{
			BanListTypes banType = card.BanInfo;
			int maxCards = 3;
			switch (banType)
			{
				case BanListTypes.Unlimited:
					maxCards = 3;
					break;
				case BanListTypes.SemiLimited:
					maxCards = 2;
					break;
				case BanListTypes.Limited:
					maxCards = 1;
					break;
				case BanListTypes.Banned:
					return false;
				default:
					maxCards = 3;
					break;
			}

			if (!CardCount.ContainsKey(card.Name))
			{
				return true;
			}

			return CardCount[card.Name] < maxCards;
		}
	}
}
