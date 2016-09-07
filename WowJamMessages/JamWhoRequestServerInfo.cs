using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamWhoRequestServerInfo", Version = 28333852u), DataContract]
	public class JamWhoRequestServerInfo
	{
		[FlexJamMember(Name = "factionGroup", Type = FlexJamType.Int32), DataMember(Name = "factionGroup")]
		public int FactionGroup
		{
			get;
			set;
		}

		[FlexJamMember(Name = "locale", Type = FlexJamType.Int32), DataMember(Name = "locale")]
		public int Locale
		{
			get;
			set;
		}

		[FlexJamMember(Name = "requesterVirtualRealmAddress", Type = FlexJamType.UInt32), DataMember(Name = "requesterVirtualRealmAddress")]
		public uint RequesterVirtualRealmAddress
		{
			get;
			set;
		}
	}
}
