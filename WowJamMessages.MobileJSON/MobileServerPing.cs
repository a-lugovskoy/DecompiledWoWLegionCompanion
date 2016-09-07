using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[DataContract]
	public class MobileServerPing
	{
		[DataMember(Name = "token")]
		public ulong Token
		{
			get;
			set;
		}

		[DataMember(Name = "reply")]
		public bool Reply
		{
			get;
			set;
		}
	}
}
