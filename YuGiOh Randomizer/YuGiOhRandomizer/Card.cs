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
		[JsonConverter(typeof(StringEnumConverter))]
		public RaceTypes Race { get; set; }

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

		/// <summary>
		/// Whether this is on the temporary ban list (where it's too situational to be used, so we need to skip using it)
		/// </summary>
		[JsonProperty]
		[DefaultValue(false)]
		public bool TempBan { get; set; }

		[JsonIgnore]
		public GeneralCardTypes GeneralCardType
		{
			get
			{
				switch (Type)
				{
					case CardTypes.NormalMonster:
					case CardTypes.EffectMonster:
					case CardTypes.FlipEffectMonster:
					case CardTypes.UnionEffectMonster:
					case CardTypes.ToonMonster:
					case CardTypes.NormalTunerMonster:
					case CardTypes.TunerMonster:
					case CardTypes.GeminiMonster:
					case CardTypes.SpiritMonster:
					case CardTypes.PendulumNormalMonster:
					case CardTypes.PendulumEffectMonster:
					case CardTypes.PendulumFlipEffectMonster:
					case CardTypes.PendulumTunerEffectMonster:
					case CardTypes.RitualEffectMonster:
					case CardTypes.RitualMonster:
						return GeneralCardTypes.Monster;

					case CardTypes.SpellCard:
						return GeneralCardTypes.Spell;

					case CardTypes.TrapCard:
						return GeneralCardTypes.Trap;

					case CardTypes.FusionMonster:
					case CardTypes.PendulumEfectFusionMonster:
						return GeneralCardTypes.Fusion;

					case CardTypes.SynchroMonster:
					case CardTypes.SynchroPendulumEffectMonster:
					case CardTypes.SynchroTunerMonster:
						return GeneralCardTypes.Synchro;

					case CardTypes.XYZMonster:
					case CardTypes.XYZPendulumEffectMonster:
						return GeneralCardTypes.Xyz;


					case CardTypes.LinkMonster:
						return GeneralCardTypes.Link;

					default:
						throw new Exception("Tried to parse a non-existant card type somehow!");
				}
			}
		}

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
		public bool IsPendulum
		{
			get
			{
				return new List<CardTypes>()
				{
					CardTypes.PendulumEffectMonster,
					CardTypes.PendulumFlipEffectMonster,
					CardTypes.PendulumNormalMonster,
					CardTypes.PendulumTunerEffectMonster,
					CardTypes.SynchroPendulumEffectMonster,
					CardTypes.XYZPendulumEffectMonster
				}.Contains(Type);
			}
		}

		[JsonIgnore]
		public bool IsTuner
		{
			get
			{
				return new List<CardTypes>()
				{
					CardTypes.NormalTunerMonster,
					CardTypes.PendulumTunerEffectMonster,
					CardTypes.SynchroTunerMonster,
					CardTypes.TunerMonster
				}.Contains(Type);
			}
		}

		[JsonIgnore]
		public DeckTypes DeckType
		{
			get
			{
				switch (GeneralCardType)
				{
					case GeneralCardTypes.Monster:
					case GeneralCardTypes.Spell:
					case GeneralCardTypes.Trap:
					case GeneralCardTypes.RandomMain:
						return DeckTypes.Main;

					case GeneralCardTypes.Fusion:
					case GeneralCardTypes.Synchro:
					case GeneralCardTypes.Xyz:
					case GeneralCardTypes.Link:
					case GeneralCardTypes.RandomExtra:
						return DeckTypes.Extra;

					default:
						throw new ArgumentException("Tried to get deck type for an unknown card type somehow!");
				}
			}
		}

		/// <summary>
		/// Make the ToString return the name for easy debugging
		/// </summary>
		/// <returns />
		public override string ToString()
		{
			string generalCardType = Enum.GetName(typeof(GeneralCardTypes), GeneralCardType);
			string levelString = Level > 0 ? $"; Level: {Level}" : "";
			return $"{generalCardType}: {Name}{levelString}";
		}
	}
}
