using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileItemStats", Version = 28333852u), DataContract]
	public class MobileItemStats
	{
		[FlexJamMember(Name = "itemDelay", Type = FlexJamType.Int32), DataMember(Name = "itemDelay")]
		public int ItemDelay
		{
			get;
			set;
		}

		[FlexJamMember(Name = "dpr", Type = FlexJamType.Float), DataMember(Name = "dpr")]
		public float Dpr
		{
			get;
			set;
		}

		[FlexJamMember(Name = "effectiveArmor", Type = FlexJamType.Int32), DataMember(Name = "effectiveArmor")]
		public int EffectiveArmor
		{
			get;
			set;
		}

		[FlexJamMember(Name = "weaponSpeed", Type = FlexJamType.Float), DataMember(Name = "weaponSpeed")]
		public float WeaponSpeed
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemLevel", Type = FlexJamType.Int32), DataMember(Name = "itemLevel")]
		public int ItemLevel
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "bonusStat", Type = FlexJamType.Struct), DataMember(Name = "bonusStat")]
		public MobileItemBonusStat[] BonusStat
		{
			get;
			set;
		}

		[FlexJamMember(Name = "requiredLevel", Type = FlexJamType.Int32), DataMember(Name = "requiredLevel")]
		public int RequiredLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "maxDamage", Type = FlexJamType.Int32), DataMember(Name = "maxDamage")]
		public int MaxDamage
		{
			get;
			set;
		}

		[FlexJamMember(Name = "minDamage", Type = FlexJamType.Int32), DataMember(Name = "minDamage")]
		public int MinDamage
		{
			get;
			set;
		}

		[FlexJamMember(Name = "quality", Type = FlexJamType.Int32), DataMember(Name = "quality")]
		public int Quality
		{
			get;
			set;
		}
	}
}
