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
		/// The deck settings to use when generating this deck
		/// </summary>
		private DeckDistributionSetting DeckSettings { get; set; }

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
		public RandomDeck(DeckDistributionSetting deckSettings)
		{
			DeckSettings = deckSettings;
			Generate();
			Validate();

			// The settings will be changed if values are removed from lists - so, refresh it for now
			// TODO: do this in such a way that the original settings don't get modified
			Program.RefreshDeckDistributionSettings();
		}

		/// <summary>
		/// Validates the deck sizes, etc. to ensure things don't break
		/// </summary>
		public void Validate()
		{
			if (DeckSettings.MainDeckAddRandomCardsIfNeeded && MainDeckSize != MainDeckCards.Count)
			{
				throw new Exception($"Main deck not the expected size: {MainDeckCards.Count} / {MainDeckSize}");
			}

			if (DeckSettings.ExtraDeckAddRandomCardsIfNeeded && ExtraDeckSize != ExtraDeckCards.Count)
			{
				throw new Exception($"Extra deck not the expected size:  {ExtraDeckCards.Count} / {ExtraDeckSize}");
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
			Log.WriteLine("New random deck creation started.");
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
			int mainDeckSetingsSize = DeckSettings.MainDeckSize.GetRandomValue();
			MainDeckSize = Math.Clamp(mainDeckSetingsSize, MinMainDeckSize, MaxMainDeckSize);

			int extraDeckSettingsSize = DeckSettings.ExtraDeckSize.GetRandomValue();
			ExtraDeckSize = Math.Clamp(extraDeckSettingsSize, MinExtraDeckSize, MaxExtraDeckSize);
		}

		/// <summary>
		/// Goes through and adds all the cards from the set tasks
		/// If it reaches the deck size limit, it will stop where it left off
		/// </summary>
		public void AddCardsFromTasks()
		{
			foreach (DeckCreationTask task in DeckSettings.DeckCreationTasks)
			{
				Log.WriteLine($"Adding cards from task: {task}");
				if (task.CardRange == null)
				{
					throw new Exception("Error in deck JSON: Task found without a card range defined!");
				}
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
			if (DeckSettings.MainDeckAddRandomCardsIfNeeded && mainDeckCardsRemaining > 0)
			{
				Log.WriteLine("Not enough tasks to fill main deck - adding random cards.");
				AddCardsFromTask(DeckCreationTask.GetTaskForRandomMainCards(mainDeckCardsRemaining), true);
			}

			int extraDeckCardsRemaining = ExtraDeckSize - ExtraDeckCards.Count;
			if (DeckSettings.ExtraDeckAddRandomCardsIfNeeded && extraDeckCardsRemaining > 0)
			{
				Log.WriteLine("Not enough tasks to fill extra deck - adding random cards.");
				AddCardsFromTask(DeckCreationTask.GetTaskForRandomExtraCards(extraDeckCardsRemaining), true);
			}
		}

		/// <summary>
		/// Adds the cards from the given task
		/// </summary>
		/// <param name="task">The task</param>
		/// <param name="overrideMainFilter">Used by the fallback random cards to add</param>
		private void AddCardsFromTask(DeckCreationTask task, bool overrideMainFilter = false)
		{
			List<Card> deck = task.DeckType == DeckTypes.Main ? MainDeckCards : ExtraDeckCards;
			int maxSize = task.DeckType == DeckTypes.Main ? MainDeckSize : ExtraDeckSize;

			int numberOfCardsToAdd = task.CardRange.GetRandomValue();
			List<Card> currentFilteredList;
			for (int i = 0; i < numberOfCardsToAdd && deck.Count < maxSize; i++)
			{
				//TODO: might be able to save performance by doing this less often
				List<Card> cardsToChooseFrom = GetCardsToChooseFrom(task);
				if (!overrideMainFilter)
                {
					cardsToChooseFrom = DeckSettings.MainFilter?.ApplyFilter(cardsToChooseFrom) ?? cardsToChooseFrom;
				}
				
				currentFilteredList = task.FilterNames(cardsToChooseFrom);

				bool cardAddResults = AddCard(currentFilteredList, deck);
				string currentFilter = task.FilterSet?.CurrentFilter?.ToString() ?? "";
				task.OnAddCardAttempt(cardAddResults);

				if (task.IsPatternExhausted || task.ShouldExitNow)
				{
					if (task.IsPatternExhausted)
					{
						LogNameFilterError(currentFilter, i);
					}
					Log.WriteLine($"- Could only add {i} card(s) for this task because too many cards were filtered.");
					break;
				}

				// Retry this index again - this means that the name filter failed, but we're trying the next pattern!
				if (!cardAddResults)
				{
					if (task.FilterSet == null)
					{
						Log.WriteLine("- ERROR: Somehow, we're retrying to add a card when there's no filter set!");
					}

					LogNameFilterError(currentFilter, i);
					i--;
				}
			}
		}

		private void LogNameFilterError(string currentNameFilter, int currentIndex)
		{
			Log.WriteLine($"- Filter with name string \"{currentNameFilter}\" exhausted at index {currentIndex}!");
		}

		/// <summary>
		/// Get the filtered list of all cards to randomly choose from
		/// This will NOT filter names, as that is done in the loop due to how the filtering works!
		/// </summary>
		/// <param name="task">The task to base the filters off of</param>
		/// <returns>The card pool to randomly choose from</returns>
		private List<Card> GetCardsToChooseFrom(DeckCreationTask task)
		{
			List<Card> cardsToChooseFrom = task.DeckType == DeckTypes.Main
				? Program.CardList.MainDeckCards
				: Program.CardList.ExtraDeckCards;

			bool ignoreTypeCheck = 
				task.GeneralCardType == GeneralCardTypes.RandomMain ||
				task.GeneralCardType == GeneralCardTypes.RandomExtra;
			if (!ignoreTypeCheck)
            {
				cardsToChooseFrom = cardsToChooseFrom.Where(x => 
					x.GeneralCardType == task.GeneralCardType).ToList();
			}

			cardsToChooseFrom = task.FilterSet?.ApplyMainFilter(cardsToChooseFrom) ?? cardsToChooseFrom;
			return cardsToChooseFrom;
		}

		/// <summary>
		/// Adds a card from the given list
		/// Will respect the ban values
		/// </summary>
		/// <param name="listToChooseFrom">The list of cards to randomly choose from - will be shuffled!</param>
		/// <param name="deckToAddTo">The deck to add to</param>
		/// <returns>True if a card was added, false otherwise</returns>
		private bool AddCard(List<Card> listToChooseFrom, List<Card> deckToAddTo)
		{

			listToChooseFrom.Shuffle();
			Card cardToAdd = listToChooseFrom.FirstOrDefault(x => DoesCardPassBanCheck(x));

			if (cardToAdd == null)
			{
				return false;
			}

			if (CardCount.ContainsKey(cardToAdd.Name))
			{
				CardCount[cardToAdd.Name]++;
			}

			else
			{
				CardCount[cardToAdd.Name] = 1;
			}

			deckToAddTo.Add(cardToAdd);
			return true;
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

			if (!DeckSettings.IgnoreBanList)
			{
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
			}

			if (!CardCount.ContainsKey(card.Name))
			{
				return true;
			}

			return CardCount[card.Name] < maxCards;
		}
	}
}
