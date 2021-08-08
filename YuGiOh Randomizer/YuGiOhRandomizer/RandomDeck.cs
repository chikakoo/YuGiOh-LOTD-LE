using System.Collections.Generic;
using System.Linq;

namespace YuGiOhRandomizer
{
	public class RandomDeck
	{
		private const int NumberOfLevel7PlusMonsters = 2;
		private const int NumberOfLevel5Or6Monsters = 2;
		private const int NumberOfLevel4Monsters = 9;
		private const int NumberOfLevel3Monsters = 4;
		private const int NumberOfLevel2Monsters = 3;
		private const int NumberOfLevel1Monsters = 2;
		private const int NumberOfSpells = 9;
		private const int NumberOfTraps = 9;
		private const int NumberOfExtraDeckMonsters = 15;

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
		/// Constructor - creates the object with a random deck
		/// </summary>
		public RandomDeck()
		{
			RandomizeDeck();
		}

		/// <summary>
		/// Randomizes the deck
		/// </summary>
		private void RandomizeDeck()
		{
			AddMonsters();
			AddSpells();
			AddTraps();
			MainDeckCards = MainDeckCards.OrderBy(x => x.Name).ToList();

			AddExtraDeckMonsters();
			ExtraDeckCards = ExtraDeckCards.OrderBy(x => x.Name).ToList();
		}

		/// <summary>
		/// Adds all the monsters according to the level restrictions
		/// </summary>
		private void AddMonsters()
		{
			AddMonstersInLevelRange(NumberOfLevel7PlusMonsters, 7, 99);
			AddMonstersInLevelRange(NumberOfLevel5Or6Monsters, 5, 6);
			AddMonstersInLevelRange(NumberOfLevel4Monsters, 4);
			AddMonstersInLevelRange(NumberOfLevel3Monsters, 3);
			AddMonstersInLevelRange(NumberOfLevel2Monsters, 2);
			AddMonstersInLevelRange(NumberOfLevel1Monsters, 1);
		}

		/// <summary>
		/// Adds monsters within the given level range (both inclusive)
		/// </summary>
		/// <param name="numberToAdd">The number of monsters to add</param>
		/// <param name="min">The minimum level</param>
		/// <param name="max">The maximum level</param>
		private void AddMonstersInLevelRange(int numberToAdd, int min, int max)
		{
			for (int i = 0; i < numberToAdd; i++)
			{
				AddCard(
					Program.CardList.NonSpecificMonsters.Where(x => x.Level >= min && x.Level <= max).ToList(),
					MainDeckCards
				);
			}
		}

		/// <summary>
		/// Adds monsters of the given level
		/// </summary>
		/// <param name="numberToAdd">The number to add</param>
		/// <param name="level">The level of monsters to add</param>
		private void AddMonstersInLevelRange(int numberToAdd, int level)
		{
			AddMonstersInLevelRange(numberToAdd, level, level);
		}

		/// <summary>
		/// Adds spells to the deck
		/// </summary>
		private void AddSpells()
		{
			for (int i = 0; i < NumberOfSpells; i++)
			{
				AddCard(Program.CardList.Spells, MainDeckCards);
			}
		}

		/// <summary>
		/// Adds traps to the deck
		/// </summary>
		private void AddTraps()
		{
			for (int i = 0; i < NumberOfTraps; i++)
			{
				AddCard(Program.CardList.Traps, MainDeckCards);
			}
		}

		/// <summary>
		/// Adds 15 completely random extra deck monsters
		/// Eventually this will be more reasonable!
		/// </summary>
		private void AddExtraDeckMonsters()
		{
			for (int i = 0; i < NumberOfExtraDeckMonsters; i++)
			{
				AddCard(Program.CardList.ExtraDeckMonsters, ExtraDeckCards);
			}
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
