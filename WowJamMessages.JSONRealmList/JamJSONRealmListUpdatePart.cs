using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmListUpdatePart", Version = 28333852u), DataContract]
	public class JamJSONRealmListUpdatePart
	{
		[FlexJamMember(Name = "wowRealmAddress", Type = FlexJamType.UInt32), DataMember(Name = "wowRealmAddress")]
		public uint WowRealmAddress
		{
			get;
			set;
		}

		[FlexJamMember(Name = "update", Type = FlexJamType.Struct), DataMember(Name = "update")]
		public JamJSONRealmEntry Update
		{
			get;
			set;
		}

		[FlexJamMember(Name = "deleting", Type = FlexJamType.Bool), DataMember(Name = "deleting")]
		public bool Deleting
		{
			get;
			set;
		}
	}
}
