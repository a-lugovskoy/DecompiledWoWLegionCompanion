using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmListServerIPAddress", Version = 28333852u), DataContract]
	public class JamJSONRealmListServerIPAddress
	{
		[FlexJamMember(Name = "ip", Type = FlexJamType.SockAddr), DataMember(Name = "ip")]
		public string Ip
		{
			get;
			set;
		}

		[FlexJamMember(Name = "port", Type = FlexJamType.UInt16), DataMember(Name = "port")]
		public ushort Port
		{
			get;
			set;
		}
	}
}
