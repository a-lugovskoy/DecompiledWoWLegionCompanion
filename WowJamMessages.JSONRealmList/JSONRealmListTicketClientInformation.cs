using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamMessage(Id = 15035, Name = "JSONRealmListTicketClientInformation", Version = 28333852u), DataContract]
	public class JSONRealmListTicketClientInformation
	{
		[FlexJamMember(Name = "info", Type = FlexJamType.Struct), DataMember(Name = "info")]
		public JamJSONRealmListTicketClientInformation Info
		{
			get;
			set;
		}
	}
}
