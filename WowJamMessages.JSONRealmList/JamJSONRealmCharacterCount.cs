using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmCharacterCount", Version = 28333852u), DataContract]
	public class JamJSONRealmCharacterCount
	{
		[FlexJamMember(Name = "wowRealmAddress", Type = FlexJamType.UInt32), DataMember(Name = "wowRealmAddress")]
		public uint WowRealmAddress
		{
			get;
			set;
		}

		[FlexJamMember(Name = "count", Type = FlexJamType.UInt8), DataMember(Name = "count")]
		public byte Count
		{
			get;
			set;
		}
	}
}
