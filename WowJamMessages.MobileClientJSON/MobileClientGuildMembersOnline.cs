using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4845, Name = "MobileClientGuildMembersOnline", Version = 28333852u), DataContract]
	public class MobileClientGuildMembersOnline
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "members", Type = FlexJamType.Struct), DataMember(Name = "members")]
		public MobileGuildMember[] Members
		{
			get;
			set;
		}
	}
}
