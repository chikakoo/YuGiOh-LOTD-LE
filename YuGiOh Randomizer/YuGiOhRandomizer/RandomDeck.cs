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

		/// <summary>
		/// Used to enforce the card limit
		/// </summary>
		private Dictionary<string, int> CardCount { get; set; } = new Dictionary<string, int>();

		/// <summary>
		/// The cards in the deck
		/// </summary>
		public List<Card> Cards { get; set; } = new List<Card>();

		/// <summary>
		/// The cards in the main deck
		/// </summary>
		public List<Card> MainDeckCards
		{
			get
			{
				return Cards.Where(x =>
					x.IsNonSpecificMonster || x.IsSpellCard || x.IsTrapCard
				).ToList();
			}
		}

		/// <summary>
		/// The cards in the extra deck
		/// </summary>
		public List<Card> ExtraDeckCards
		{
			get
			{
				return Cards.Where(x =>
					x.IsExtraDeckCard
				).ToList();
			}
		}

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
			Cards = Cards.OrderBy(x => x.Name).ToList();
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
				AddCard(Program.CardList.NonSpecificMonsters.Where(x => x.Level >= min && x.Level <= max).ToList());
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
				AddCard(Program.CardList.Spells);
			}
		}

		/// <summary>
		/// Adds traps to the deck
		/// </summary>
		private void AddTraps()
		{
			for (int i = 0; i < NumberOfTraps; i++)
			{
				AddCard(Program.CardList.Traps);
			}
		}

		/// <summary>
		/// Adds a card from the given list
		/// Will respect the ban values
		/// </summary>
		/// <param name="listToChooseFrom"></param>
		private void AddCard(List<Card> listToChooseFrom)
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


			Cards.Add(cardToAdd);
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
