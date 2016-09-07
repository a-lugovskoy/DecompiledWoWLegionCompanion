using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamRequestAILock", Version = 28333852u), DataContract]
	public class JamRequestAILock
	{
		[FlexJamMember(Name = "lockReason", Type = FlexJamType.UInt32), DataMember(Name = "lockReason")]
		public uint LockReason
		{
			get;
			set;
		}

		[FlexJamMember(Name = "ticketGUID", Type = FlexJamType.WowGuid), DataMember(Name = "ticketGUID")]
		public string TicketGUID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "lockResourceGUID", Type = FlexJamType.WowGuid), DataMember(Name = "lockResourceGUID")]
		public string LockResourceGUID
		{
			get;
			set;
		}
	}
}
