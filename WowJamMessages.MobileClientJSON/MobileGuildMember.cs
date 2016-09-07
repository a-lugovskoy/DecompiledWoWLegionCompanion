using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileGuildMember", Version = 28333852u), DataContract]
	public class MobileGuildMember
	{
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string Guid
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
	}
}
