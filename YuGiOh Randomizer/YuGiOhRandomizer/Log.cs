using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// Functions to log what happened the last time the randomizer was ran
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// The file name of the log
		/// </summary>
		private const string LogFileName = "LastRunLog.txt";

		/// <summary>
		/// The current text in the log
		/// </summary>
		private static StringBuilder Text { get; set; } = new StringBuilder();

		/// <summary>
		/// Writes a line to the log
		/// </summary>
		/// <param name="line">The line to write</param>
		public static void WriteLine(string line = "")
		{
			Text.Append($"{line}\n");
		}

		/// <summary>
		/// Writes the given deck to the log
		/// </summary>
		/// <param name="deck">The deck</param>
		/// <param name="titleOfDeck">The title of the deck</param>
		public static void WriteDeck(RandomDeck deck, string titleOfDeck)
		{
			WriteDeck(deck.MainDeckCards, $"{titleOfDeck} (Main)");

			if (deck.ExtraDeckCards.Count > 0)
			{
				WriteDeck(deck.ExtraDeckCards, $"{titleOfDeck} (Extra)");
			}
		}

		/// <summary>
		/// Writes the given deck to the log
		/// </summary>
		/// <param name="deck">The deck</param>
		/// <param name="titleOfDeck">The title of the deck</param>
		public static void WriteDeck(List<Card> deck, string titleOfDeck)
		{
			WriteLine();
			WriteLine(titleOfDeck);
			WriteLine("----------");

			foreach (Card card in deck)
			{
				WriteLine(card.ToString());
			}

			WriteLine();
		}

		/// <summary>
		/// Saves the log to a file
		/// </summary>
		public static void SaveToFile()
		{
			string thisDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = $@"{thisDirectory}\{LogFileName}";

			File.WriteAllText(filePath, Text.ToString());
		}
	}
}
