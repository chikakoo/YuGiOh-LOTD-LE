namespace YuGiOhRandomizer
{
	public class Program
	{
		public static CardList CardList { get; set; }
		public static DeckSettings DeckSettings { get; set; }

		public static void Main(string[] args)
		{
			DeckSettings = new DeckSettings(args);
			CardList = CardList.GetCardListInstance();
			new DeckFileCreator().CreateAndSaveDecks();
		}
	}
}
