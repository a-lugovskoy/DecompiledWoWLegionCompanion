using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[FlexJamMessage(Id = 4741, Name = "MobileServerDevLogin", Version = 28333852u), DataContract]
	public class MobileServerDevLogin
	{
		[FlexJamMember(Name = "locale", Type = FlexJamType.String), DataMember(Name = "locale")]
		public string Locale
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

		[FlexJamMember(Name = "characterID", Type = FlexJamType.WowGuid), DataMember(Name = "characterID")]
		public string CharacterID
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

		[FlexJamMember(Name = "bnetAccount", Type = FlexJamType.WowGuid), DataMember(Name = "bnetAccount")]
		public string BnetAccount
		{
			get;
			set;
		}
	}
}
