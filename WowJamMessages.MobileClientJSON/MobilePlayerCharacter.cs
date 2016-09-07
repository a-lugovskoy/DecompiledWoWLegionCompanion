using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobilePlayerCharacter", Version = 28333852u), DataContract]
	public class MobilePlayerCharacter
	{
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string Guid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "charLevel", Type = FlexJamType.UInt8), DataMember(Name = "charLevel")]
		public byte CharLevel
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

		[FlexJamMember(Name = "status", Type = FlexJamType.Int32), DataMember(Name = "status")]
		public int Status
		{
			get;
			set;
		}

		[FlexJamMember(Name = "charClass", Type = FlexJamType.UInt8), DataMember(Name = "charClass")]
		public byte CharClass
		{
			get;
			set;
		}

		[FlexJamMember(Name = "charRace", Type = FlexJamType.UInt8), DataMember(Name = "charRace")]
		public byte CharRace
		{
			get;
			set;
		}
	}
}
