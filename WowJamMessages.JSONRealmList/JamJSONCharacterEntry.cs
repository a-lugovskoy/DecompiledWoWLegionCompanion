using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONCharacterEntry", Version = 28333852u), DataContract]
	public class JamJSONCharacterEntry
	{
		[FlexJamMember(Name = "raceID", Type = FlexJamType.UInt8), DataMember(Name = "raceID")]
		public byte RaceID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "hasMobileAccess", Type = FlexJamType.Bool), DataMember(Name = "hasMobileAccess")]
		public bool HasMobileAccess
		{
			get;
			set;
		}

		[FlexJamMember(Name = "experienceLevel", Type = FlexJamType.UInt8), DataMember(Name = "experienceLevel")]
		public byte ExperienceLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "playerGuid", Type = FlexJamType.WowGuid), DataMember(Name = "playerGuid")]
		public string PlayerGuid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "lastActiveTime", Type = FlexJamType.Int32), DataMember(Name = "lastActiveTime")]
		public int LastActiveTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "virtualRealmAddress", Type = FlexJamType.UInt32), DataMember(Name = "virtualRealmAddress")]
		public uint VirtualRealmAddress
		{
			get;
			set;
		}

		[FlexJamMember(Name = "name", Type = FlexJamType.String), DataMember(Name = "name")]
		public string Name
		{
			get;
			set;
		}

		[FlexJamMember(Name = "classID", Type = FlexJamType.UInt8), DataMember(Name = "classID")]
		public byte ClassID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "sexID", Type = FlexJamType.UInt8), DataMember(Name = "sexID")]
		public byte SexID
		{
			get;
			set;
		}
	}
}
