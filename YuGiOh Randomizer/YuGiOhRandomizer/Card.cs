using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace YuGiOhRandomizer
{
	public class Card
	{
		[JsonProperty]
		public int Id { get; set; }

		[JsonIgnore]
		public byte FirstCardIdByte
		{
			get
			{
				return BitConverter.GetBytes(Id)[0];
			}
		}

		[JsonIgnore]
		public byte SecondCardIdByte
		{
			get
			{
				return BitConverter.GetBytes(Id)[1];
			}
		}

		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public CardTypes Type { get; set; }

		[JsonProperty]
		public string Attribute { get; set; }

		[JsonProperty]
		public int Level { get; set; }

		[JsonProperty]
		public string Race { get; set; }

		[JsonProperty("atk")]
		public int Attack { get; set; }

		[JsonProperty("def")]
		public int Defense { get; set; }

		[JsonProperty]
		public string Archetype { get; set; }

		[JsonProperty("banlist_info")]
		[JsonConverter(typeof(StringEnumConverter))]
		[DefaultValue(BanListTypes.Unlimited)]
		public BanListTypes BanInfo { get; set; }

		[JsonIgnore]
		public bool IsNonSpecificMonster
		{
			get
			{
				return new List<CardTypes>()
				{
					CardTypes.EffectMonster,
					CardTypes.FlipEffectMonster,
					CardTypes.GeminiMonster,
					CardTypes.NormalMonster,
					CardTypes.NormalTunerMonster,
					CardTypes.PendulumEffectMonster,
					CardTypes.PendulumFlipEffectMonster,
					CardTypes.PendulumNormalMonster,
					CardTypes.PendulumTunerEffectMonster,
					CardTypes.SpiritMonster,
					CardTypes.TunerMonster,
					CardTypes.UnionEffectMonster
				}.Contains(Type);
			}
		}

		[JsonIgnore]
		public bool IsSpellCard
		{
			get
			{
				return Type == CardTypes.SpellCard;
			}
		}

		[JsonIgnore]
		public bool IsTrapCard
		{
			get
			{
				return Type == CardTypes.TrapCard;
			}
		}

		[JsonIgnore]
		public bool IsExtraDeckCard
		{
			get
			{
				return new List<CardTypes>()
				{
					CardTypes.FusionMonster,
					CardTypes.LinkMonster,
					CardTypes.PendulumEfectFusionMonster,
					CardTypes.SynchroMonster,
					CardTypes.SynchroPendulumEffectMonster,
					CardTypes.SynchroTunerMonster,
					CardTypes.XYZMonster,
					CardTypes.XYZPendulumEffectMonster
				}.Contains(Type);
			}
		}

		/// <summary>
		/// Make the ToString return the name for easy debugging
		/// </summary>
		/// <returns />
		public override string ToString()
		{
			return Name;
		}
	}
}
