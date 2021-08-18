using System;
using System.Collections.Generic;
using System.IO;

namespace YuGiOh_Save_Deck_Extractor
{
	public enum ExitCode : int
	{
		Success = 0,
		Error = 1
	}

	public class Program
	{
		private static Settings Settings { get; set; }
		private static readonly int[] Empty = new int[0];

		public static int Main(string[] args)
		{
			Settings = new Settings(args);

			byte[] savegame = File.ReadAllBytes(Settings.SaveGameLocation);
			if (savegame.Length == 0)
			{
				throw new FileNotFoundException("Error: could not get the savedata!");
			}
			byte[] deckData = ExtractDeckFromSaveData(savegame);
			File.WriteAllBytes($"{Settings.PackingScriptLocation}\\YGO_2020\\decks.zib\\{Settings.DeckToReplace}.ydc", deckData);

			Console.WriteLine("Data successfully extracted!");

			return (int)ExitCode.Success;
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

		/// <summary>
		/// Extracts the deck from the save data - see the DeckToExtract constant
		/// 
		/// This is based off of the following file - full credits to thomasneff for this
		/// https://github.com/thomasneff/YGOLOTDPatchDraft/blob/master/YGOLOTDPatchDraft/FileUtilities.cs
		/// </summary>
		/// <param name="savegame">The byte array of the save game</param>
		/// <returns>byte array in LOTD Deck format</returns>
		public static byte[] ExtractDeckFromSaveData(byte[] savegame)
		{
			byte[] searchPattern = GetSearchPattern(Settings.DeckToExtract);

			var locations = savegame.Locate(searchPattern);

			// Take the first one (The second one is the chosen draft cards, I think. Haven't verified that yet.)
			if (locations.Length == 0)
			{
				throw new ArgumentException("Error: Couldn't find the location of the deck in your savegame.dat!");
			}

			const int deckNameByteLength = 66;
			const int MaxMainDeckCards = 60;
			const int MaxExtraDeckCards = 15;

			// Offset until the number of main deck cards starts
			int byteOffset = locations[0] + deckNameByteLength;
			int numberOfMainDeckCards = (savegame[byteOffset + 1] << 8) + savegame[byteOffset];
			byteOffset += 2;
			int numberOfExtraDeckCards = (savegame[byteOffset + 1] << 8) + savegame[byteOffset];
			byteOffset += 2;
			int numberOfSideDeckCardsards = (savegame[byteOffset + 1] << 8) + savegame[byteOffset];
			byteOffset += 2;
			int startDeckOffset = byteOffset;
			if (numberOfMainDeckCards == 0)
			{
				throw new ArgumentException("Error: Deck is empty!");
			}

			List<byte> ydcDeckFormat = new List<byte>();
			long headerByte = 25740;
			byte[] ydcBytes = new byte[0];

			using (var memWriter = new MemoryStream())
			{
				using (var writer = new BinaryWriter(memWriter))
				{
					// I don't know what the first 8 bytes do, just write what they contain by default
					writer.Write(headerByte);

					// Write main deck
					writer.Write((short)numberOfMainDeckCards);
					for (; byteOffset < startDeckOffset + (numberOfMainDeckCards * 2); byteOffset += 2)
					{
						writer.Write(savegame[byteOffset]);
						writer.Write(savegame[byteOffset + 1]);
					}

					// Write extra deck
					byteOffset += (MaxMainDeckCards - numberOfMainDeckCards) * 2;
					writer.Write((short)numberOfExtraDeckCards);
					for (; byteOffset < startDeckOffset + ((MaxMainDeckCards + numberOfExtraDeckCards) * 2); byteOffset += 2)
					{
						writer.Write(savegame[byteOffset]);
						writer.Write(savegame[byteOffset + 1]);
					}

					//Write side deck
					byteOffset += (MaxExtraDeckCards - numberOfExtraDeckCards) * 2;
					writer.Write((short)numberOfSideDeckCardsards);
					for (; byteOffset < startDeckOffset + ((MaxMainDeckCards + MaxExtraDeckCards + numberOfSideDeckCardsards) * 2); byteOffset += 2)
					{
						writer.Write(savegame[byteOffset]);
						writer.Write(savegame[byteOffset + 1]);
					}
				}
				ydcBytes = memWriter.ToArray();
			}

			return ydcBytes;
		}
	}
}
