namespace YuGiOhRandomizer
{
	public class DeckSettings
	{
		/// <summary>
		/// The location of the savegame.dat in the Steam directory
		/// </summary>
		public string SaveGameLocation { get; set; }

		/// <summary>
		/// The location of the scripts to pack and unpack the save data
		/// </summary>
		public string PackingScriptLocation { get; set; }

		/// <summary>
		/// The opponent's deck name to replace (e.g. "1classic_hard_bandit")
		/// </summary>
		public string OpponentDeckToReplace { get; set; }

		/// <summary>
		/// Your deck to replace - ensure that you have a custom deck with this name (e.g. "Random")
		/// </summary>
		public string PlayerDeckToReplace { get; set; }

		/// <summary>
		/// The constructor
		/// </summary>
		/// <param name="args">The arguments passed in from the command line</param>
		public DeckSettings(string[] args)
		{
			SaveGameLocation = args[0];
			PackingScriptLocation = args[1];
			OpponentDeckToReplace = args[2];
			PlayerDeckToReplace = args[3];
		}
	}
}
