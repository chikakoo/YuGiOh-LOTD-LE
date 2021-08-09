using System.Runtime.Serialization;

namespace YuGiOhRandomizer
{
	/// <summary>
	/// Used with deck distributions - contains the general types of cards
	/// </summary>
	public enum GeneralCardTypes
	{
		// Main Decks
		[EnumMember(Value = "Monster")]
		Monster,

		[EnumMember(Value = "Spell")]
		Spell,

		[EnumMember(Value = "Trap")]
		Trap,

		// Extra Decks
		[EnumMember(Value = "Fusion")]
		Fusion,

		[EnumMember(Value = "Synchro")]
		Synchro,

		[EnumMember(Value = "Xyz")]
		Xyz,

		[EnumMember(Value = "Link")]
		Link,

		// Used to mean any card type in that kind of deck
		[EnumMember(Value = "RandomMain")]
		RandomMain,

		[EnumMember(Value = "RandomExtra")]
		RandomExtra
	}
}
