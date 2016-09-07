using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamPartyMemberPetState", Version = 28333852u), DataContract]
	public class JamPartyMemberPetState
	{
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string Guid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "health", Type = FlexJamType.Int32), DataMember(Name = "health")]
		public int Health
		{
			get;
			set;
		}

		[FlexJamMember(Name = "maxHealth", Type = FlexJamType.Int32), DataMember(Name = "maxHealth")]
		public int MaxHealth
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

		[FlexJamMember(ArrayDimensions = 1, Name = "auras", Type = FlexJamType.Struct), DataMember(Name = "auras")]
		public JamPartyMemberAuraState[] Auras
		{
			get;
			set;
		}

		[FlexJamMember(Name = "displayID", Type = FlexJamType.Int32), DataMember(Name = "displayID")]
		public int DisplayID
		{
			get;
			set;
		}

		public JamPartyMemberPetState()
		{
			this.Guid = "0000000000000000";
		}
	}
}
