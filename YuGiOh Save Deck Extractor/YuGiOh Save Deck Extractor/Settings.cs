namespace YuGiOh_Save_Deck_Extractor
{
	public class Settings
	{
		public string SaveGameLocation { get; set; }
		public string PackingScriptLocation { get; set; }
		public string DeckToExtract { get; set; }
		public string DeckToReplace { get; set; }

		/// <summary>
		/// The constructor
		/// </summary>
		/// <param name="args">The arguments passed in from the command line</param>
		public Settings(string[] args)
		{
			SaveGameLocation = args[0];
			PackingScriptLocation = args[1];
			DeckToExtract = args[2];
			DeckToReplace = args[3];
		}
	}
}
