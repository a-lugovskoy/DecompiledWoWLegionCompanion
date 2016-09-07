using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[DataContract]
	public enum MobileCharacterStatus
	{
		[EnumMember]
		MOBILE_CHAR_STATUS_OKAY = 0,
		[EnumMember]
		MOBILE_CHAR_STATUS_NEED_QUEST = 1,
		[EnumMember]
		MOBILE_CHAR_STATUS_LOCKED = 2,
		[EnumMember]
		MOBILE_CHAR_STATUS_LOCKED_BILLING = 3,
		[EnumMember]
		MOBILE_CHAR_STATUS_REVOKED_UPGRADE = 4,
		[EnumMember]
		MOBILE_CHAR_STATUS_REVOKED_TRANSACTION = 5,
		[EnumMember]
		MOBILE_CHAR_STATUS_RENAME = 6
	}
}
