using System.Runtime.Serialization;

namespace YuGiOhRandomizer
{
	public enum BanListTypes
	{
		[EnumMember(Value = "undefined")]
		Unlimited,

		[EnumMember(Value = "Limited")]
		Limited,

		[EnumMember(Value = "Semi-Limited")]
		SemiLimited,

		[EnumMember(Value = "Banned")]
		Banned
	}
}
