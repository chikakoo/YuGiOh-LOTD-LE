﻿using System;

namespace YuGiOhRandomizer
{
	public class Program
	{
		public static DeckDistributionSettings DeckDistributionSettings { get; set; }
		public static CardList CardList { get; set; }
		public static DeckSettings DeckSettings { get; set; }

		private enum ExitCode : int
		{
			Success = 0,
			Error = 1
		}

		public static int Main(string[] args)
		{
			try
			{
				DeckDistributionSettings = DeckDistributionSettings.GetSettingsInstance();
				DeckSettings = new DeckSettings(args);
				CardList = CardList.GetCardListInstance();
				new DeckFileCreator().CreateAndSaveDecks();

				Log.SaveToFile();
				return (int)ExitCode.Success;
			}

			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return (int)ExitCode.Error;
			}
		}
	}
}
