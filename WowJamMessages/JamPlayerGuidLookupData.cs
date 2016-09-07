using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamPlayerGuidLookupData", Version = 28333852u), DataContract]
	public class JamPlayerGuidLookupData
	{
		[FlexJamMember(Name = "level", Type = FlexJamType.UInt8), DataMember(Name = "level")]
		public byte Level
		{
			get;
			set;
		}

		[FlexJamMember(Name = "wowAccount", Type = FlexJamType.WowGuid), DataMember(Name = "wowAccount")]
		public string WowAccount
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "declinedNames", Type = FlexJamType.String), DataMember(Name = "declinedNames")]
		public string[] DeclinedNames
		{
			get;
			set;
		}

		[FlexJamMember(Name = "guidActual", Type = FlexJamType.WowGuid), DataMember(Name = "guidActual")]
		public string GuidActual
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

		[FlexJamMember(Name = "race", Type = FlexJamType.UInt8), DataMember(Name = "race")]
		public byte Race
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

		[FlexJamMember(Name = "sex", Type = FlexJamType.UInt8), DataMember(Name = "sex")]
		public byte Sex
		{
			get;
			set;
		}

		[FlexJamMember(Name = "isDeleted", Type = FlexJamType.Bool), DataMember(Name = "isDeleted")]
		public bool IsDeleted
		{
			get;
			set;
		}

		[FlexJamMember(Name = "bnetAccount", Type = FlexJamType.WowGuid), DataMember(Name = "bnetAccount")]
		public string BnetAccount
		{
			get;
			set;
		}

		public JamPlayerGuidLookupData()
		{
			this.DeclinedNames = new string[5];
		}
	}
}
