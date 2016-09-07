using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[DataContract]
	public class MobileServerCreateSessionRequest
	{
		[DataMember(Name = "token")]
		public ulong Token
		{
			get;
			set;
		}

		[DataMember(Name = "rSessionID")]
		public string RSessionID
		{
			get;
			set;
		}

		[DataMember(Name = "bnetAccountID")]
		public ulong BnetAccountID
		{
			get;
			set;
		}
	}
}
