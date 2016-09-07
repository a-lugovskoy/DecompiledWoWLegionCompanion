using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[DataContract]
	public enum MobileStatColor
	{
		[EnumMember]
		MOBILE_STAT_COLOR_TRIVIAL = 0,
		[EnumMember]
		MOBILE_STAT_COLOR_NORMAL = 1,
		[EnumMember]
		MOBILE_STAT_COLOR_FRIENDLY = 2,
		[EnumMember]
		MOBILE_STAT_COLOR_HOSTILE = 3,
		[EnumMember]
		MOBILE_STAT_COLOR_INACTIVE = 4,
		[EnumMember]
		MOBILE_STAT_COLOR_ERROR = 5
	}
}
