using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[FlexJamMessage(Id = 4740, Name = "MobileServerLogin", Version = 28333852u), DataContract]
	public class MobileServerLogin
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "joinTicket", Type = FlexJamType.UInt8), DataMember(Name = "joinTicket")]
		public byte[] JoinTicket
		{
			get;
			set;
		}

		[FlexJamMember(Name = "locale", Type = FlexJamType.String), DataMember(Name = "locale")]
		public string Locale
		{
			get;
			set;
		}
	}
}
