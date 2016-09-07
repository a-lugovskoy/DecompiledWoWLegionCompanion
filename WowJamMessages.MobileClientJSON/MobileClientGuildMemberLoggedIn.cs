using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4846, Name = "MobileClientGuildMemberLoggedIn", Version = 28333852u), DataContract]
	public class MobileClientGuildMemberLoggedIn
	{
		[FlexJamMember(Name = "member", Type = FlexJamType.Struct), DataMember(Name = "member")]
		public MobileGuildMember Member
		{
			get;
			set;
		}
	}
}
