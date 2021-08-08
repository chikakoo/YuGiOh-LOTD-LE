using System.Runtime.Serialization;

namespace YuGiOhRandomizer
{
	public enum RaceTypes
	{
		// Spells/Traps
		[EnumMember(Value = "Normal")]
		Normal,

		[EnumMember(Value = "Continuous")]
		Continuous,

		// Spells
		[EnumMember(Value = "Equip")]
		Equip,

		[EnumMember(Value = "Quick-Play")]
		QuickPlay,

		[EnumMember(Value = "Field")]
		Field,

		[EnumMember(Value = "Ritual")]
		Ritual,

		[EnumMember(Value = "Aqua")]
		Aqua,

		[EnumMember(Value = "Beast")]
		Beast,

		[EnumMember(Value = "Beast-Warrior")]
		BeastWarrior,

		// Traps
		[EnumMember(Value = "Counter")]
		Counter,

		// Monsters
		[EnumMember(Value = "Cyberse")]
		Cyberse,

		[EnumMember(Value = "Dinosaur")]
		Dinosaur,

		[EnumMember(Value = "Divine-Beast")]
		DivineBeast,

		[EnumMember(Value = "Dragon")]
		Dragon,

		[EnumMember(Value = "Fairy")]
		Fairy,

		[EnumMember(Value = "Fiend")]
		Fiend,

		[EnumMember(Value = "Fish")]
		Fish,

		[EnumMember(Value = "Insect")]
		Insect,

		[EnumMember(Value = "Machine")]
		Machine,

		[EnumMember(Value = "Plant")]
		Plant,

		[EnumMember(Value = "Psychic")]
		Psychic,

		[EnumMember(Value = "Pyro")]
		Pyro,

		[EnumMember(Value = "Reptile")]
		Reptile,

		[EnumMember(Value = "Rock")]
		Rock,

		[EnumMember(Value = "Sea Serpent")]
		SeaSerpent,

		[EnumMember(Value = "Spellcaster")]
		Spellcaster,

		[EnumMember(Value = "Thunder")]
		Thunder,

		[EnumMember(Value = "Warrior")]
		Warrior,

		[EnumMember(Value = "Winged Beast")]
		WingedBeast,

		[EnumMember(Value = "Wyrm")]
		Wyrm,

		[EnumMember(Value = "Zombie")]
		Zombie
	}
}
