using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamWhoEntry", Version = 28333852u), DataContract]
	public class JamWhoEntry
	{
		[FlexJamMember(Name = "guildGUID", Type = FlexJamType.WowGuid), DataMember(Name = "guildGUID")]
		public string GuildGUID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "areaID", Type = FlexJamType.Int32), DataMember(Name = "areaID")]
		public int AreaID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "guildVirtualRealmAddress", Type = FlexJamType.UInt32), DataMember(Name = "guildVirtualRealmAddress")]
		public uint GuildVirtualRealmAddress
		{
			get;
			set;
		}

		[FlexJamMember(Name = "guildName", Type = FlexJamType.String), DataMember(Name = "guildName")]
		public string GuildName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "playerData", Type = FlexJamType.Struct), DataMember(Name = "playerData")]
		public JamPlayerGuidLookupData PlayerData
		{
			get;
			set;
		}

		[FlexJamMember(Name = "isGM", Type = FlexJamType.Bool), DataMember(Name = "isGM")]
		public bool IsGM
		{
			get;
			set;
		}
	}
}
