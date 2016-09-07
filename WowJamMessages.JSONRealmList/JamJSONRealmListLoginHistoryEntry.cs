using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmListLoginHistoryEntry", Version = 28333852u), DataContract]
	public class JamJSONRealmListLoginHistoryEntry
	{
		[FlexJamMember(Name = "characterGUID", Type = FlexJamType.WowGuid), DataMember(Name = "characterGUID")]
		public string CharacterGUID
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

		[FlexJamMember(Name = "lastPlayedTime", Type = FlexJamType.Int32), DataMember(Name = "lastPlayedTime")]
		public int LastPlayedTime
		{
			get;
			set;
		}
	}
}
