using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamMessage(Id = 15034, Name = "JSONRealmListTicketIdentity", Version = 28333852u), DataContract]
	public class JSONRealmListTicketIdentity
	{
		[FlexJamMember(Name = "gameAccountID", Type = FlexJamType.UInt64), DataMember(Name = "gameAccountID")]
		public ulong GameAccountID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "gameAccountRegion", Type = FlexJamType.UInt8), DataMember(Name = "gameAccountRegion")]
		public byte GameAccountRegion
		{
			get;
			set;
		}
	}
}
