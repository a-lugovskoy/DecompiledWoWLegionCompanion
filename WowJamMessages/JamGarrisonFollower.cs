using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonFollower", Version = 28333852u), DataContract]
	public class JamGarrisonFollower
	{
		[FlexJamMember(Name = "customName", Type = FlexJamType.String), DataMember(Name = "customName")]
		public string CustomName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemLevelWeapon", Type = FlexJamType.Int32), DataMember(Name = "itemLevelWeapon")]
		public int ItemLevelWeapon
		{
			get;
			set;
		}

		[FlexJamMember(Name = "currentBuildingID", Type = FlexJamType.Int32), DataMember(Name = "currentBuildingID")]
		public int CurrentBuildingID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "zoneSupportSpellID", Type = FlexJamType.Int32), DataMember(Name = "zoneSupportSpellID")]
		public int ZoneSupportSpellID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "currentMissionID", Type = FlexJamType.Int32), DataMember(Name = "currentMissionID")]
		public int CurrentMissionID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "followerLevel", Type = FlexJamType.Int32), DataMember(Name = "followerLevel")]
		public int FollowerLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "flags", Type = FlexJamType.Int32), DataMember(Name = "flags")]
		public int Flags
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemLevelArmor", Type = FlexJamType.Int32), DataMember(Name = "itemLevelArmor")]
		public int ItemLevelArmor
		{
			get;
			set;
		}

		[FlexJamMember(Name = "xp", Type = FlexJamType.Int32), DataMember(Name = "xp")]
		public int Xp
		{
			get;
			set;
		}

		[FlexJamMember(Name = "dbID", Type = FlexJamType.UInt64), DataMember(Name = "dbID")]
		public ulong DbID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrFollowerID", Type = FlexJamType.Int32), DataMember(Name = "garrFollowerID")]
		public int GarrFollowerID
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

		[FlexJamMember(Name = "durability", Type = FlexJamType.Int32), DataMember(Name = "durability")]
		public int Durability
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "abilityID", Type = FlexJamType.Int32), DataMember(Name = "abilityID")]
		public int[] AbilityID
		{
			get;
			set;
		}

		public JamGarrisonFollower()
		{
			this.CustomName = string.Empty;
		}
	}
}
