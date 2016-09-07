using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[DataContract]
	public enum REALM_FLAG_BITS
	{
		[EnumMember]
		REALM_FLAG_BIT_VERSION_MISMATCH = 0,
		[EnumMember]
		REALM_FLAG_BIT_HIDDEN = 1,
		[EnumMember]
		REALM_FLAG_BIT_TOURNAMENT = 2
	}
}
