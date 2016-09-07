using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4847, Name = "MobileClientGuildMemberLoggedOut", Version = 28333852u), DataContract]
	public class MobileClientGuildMemberLoggedOut
	{
		[FlexJamMember(Name = "member", Type = FlexJamType.Struct), DataMember(Name = "member")]
		public MobileGuildMember Member
		{
			get;
			set;
		}
	}
}
