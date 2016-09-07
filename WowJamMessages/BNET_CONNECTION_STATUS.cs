using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[DataContract]
	public enum BNET_CONNECTION_STATUS
	{
		[EnumMember]
		BNET_CONNECTION_STATUS_NONE = 0,
		[EnumMember]
		BNET_CONNECTION_STATUS_OK = 1,
		[EnumMember]
		BNET_CONNECTION_STATUS_DISCONNECTED = 2
	}
}
