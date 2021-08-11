using System;
using System.Collections.Generic;
using System.IO;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// The parts of this that write and read the data are based off of code from his file - full credits to thomasneff for this
	/// https://github.com/thomasneff/YGOLOTDPatchDraft/blob/master/YGOLOTDPatchDraft/FileUtilities.cs
	/// </summary>
	public class DeckFileCreator
	{
		public RandomDeck OpponentDeck { get; set; }
		public RandomDeck PlayerDeck { get; set; }

		private const int MaxMainDeckCards = 60;
		private const int MaxExtraDeckCards = 15;
		private const int DeckNameByteLength = 66; // This is the all the space that's allocated to deck names

		public DeckFileCreator()
		{
			OpponentDeck = new RandomDeck();
			Log.WriteDeck(OpponentDeck, "Opponent's Deck");

			PlayerDeck = new RandomDeck();
			Log.WriteDeck(OpponentDeck, "Player's Deck");
		}

		public void CreateAndSaveDecks()
		{
			WriteOpponentDeckToDisk();
			WritePlayerDeckToSaveData();
			ChecksumFixer.FixGameSaveSignatureOnDisk();
		}

		/// <summary>
		/// Writes the opponent's deck to the script location
		/// </summary>
		private void WriteOpponentDeckToDisk()
		{
			byte[] ydcBytes = new byte[0];
			using (var memWriter = new MemoryStream())
			{
				using (var writer = new BinaryWriter(memWriter))
				{
					// Header for the ydc file - important to write this!
					long headerByte = 25740;
					writer.Write(headerByte);

					writer.Write((short)OpponentDeck.MainDeckCards.Count);
					foreach (Card card in OpponentDeck.MainDeckCards)
					{
						writer.Write(card.FirstCardIdByte);
						writer.Write(card.SecondCardIdByte);
					}

					writer.Write((short)OpponentDeck.ExtraDeckCards.Count);
					foreach (Card card in OpponentDeck.ExtraDeckCards)
					{
						writer.Write(card.FirstCardIdByte);
						writer.Write(card.SecondCardIdByte);
					}

					// For the side deck that will always be empty
					writer.Write((short)0);
				}
				ydcBytes = memWriter.ToArray();
			}


			string path = $@"{Program.DeckSettings.PackingScriptLocation}\YGO_2020\decks.zib\{Program.DeckSettings.OpponentDeckToReplace}.ydc";
			File.WriteAllBytes(path, ydcBytes);

			Log.WriteLine($"Wrote oppoent's deck to: {path}");
		}

		/// <summary>
		/// Writes the player deck to the save data
		/// </summary>
		private void WritePlayerDeckToSaveData()
		{
			byte[] savegame = File.ReadAllBytes(Program.DeckSettings.SaveGameLocation);
			if (savegame.Length == 0)
			{
				throw new FileNotFoundException("Error: Could not get the savedata!");
			}

			byte[] searchPattern = GetSearchPattern(Program.DeckSettings.PlayerDeckToReplace);
			var locations = savegame.Locate(searchPattern);
			if (locations.Length == 0)
			{
				throw new ArgumentException("Error: Couldn't find the location of the deck in your savegame.dat!");
			}

			// Set the offset to the place right after the deck name
			int byteOffset = locations[0] + DeckNameByteLength;

			// Set the data for the deck sizes
			int numberOfMainDeckCards = PlayerDeck.MainDeckCards.Count;
			savegame[byteOffset++] = (byte)numberOfMainDeckCards;
			savegame[byteOffset++] = 0;

			int numberOfExtraDeckCards = PlayerDeck.ExtraDeckCards.Count;
			savegame[byteOffset++] = (byte)numberOfExtraDeckCards;
			savegame[byteOffset++] = 0;

			// This is for the side deck, which we will probably never need to use!
			savegame[byteOffset++] = 0;
			savegame[byteOffset++] = 0;

			// Write the main deck
			for (int i = 0; i < MaxMainDeckCards; i++)
			{
				if (i < numberOfMainDeckCards)
				{
					Card currentCard = PlayerDeck.MainDeckCards[i];
					savegame[byteOffset++] = currentCard.FirstCardIdByte;
					savegame[byteOffset++] = currentCard.SecondCardIdByte;
				}

				else
				{
					savegame[byteOffset++] = 0;
					savegame[byteOffset++] = 0;
				}
			}

			// Write the extra deck
			for (int i = 0; i < MaxExtraDeckCards; i++)
			{
				if (i < numberOfExtraDeckCards)
				{
					Card currentCard = PlayerDeck.ExtraDeckCards[i];
					savegame[byteOffset++] = currentCard.FirstCardIdByte;
					savegame[byteOffset++] = currentCard.SecondCardIdByte;
				}

				else
				{
					savegame[byteOffset++] = 0;
					savegame[byteOffset++] = 0;
				}
			}


			File.WriteAllBytes(Program.DeckSettings.SaveGameLocation, savegame);
			Log.WriteLine($"Wrote player's deck to: {Program.DeckSettings.SaveGameLocation}");
		}

		/// <summary>
		/// Converts a string into an array of ASCII byte values with 0x00
		/// in between each entry (excludes the last value)
		/// </summary>
		/// <param name="deckName">The string to convert</param>
		/// <returns/>
		public static byte[] GetSearchPattern(string deckName)
		{
			var searchPattern = new List<byte>();
			foreach (char character in deckName)
			{
				searchPattern.Add(Convert.ToByte(character));
				searchPattern.Add(0x00);
			}
			searchPattern.RemoveAt(searchPattern.Count - 1); // Remove the last 0x00

			return searchPattern.ToArray();
		}
	}
}
