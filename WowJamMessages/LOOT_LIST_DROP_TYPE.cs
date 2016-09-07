using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[DataContract]
	public enum LOOT_LIST_DROP_TYPE
	{
		[EnumMember]
		LOOT_LIST_DROP_NORMAL = 0,
		[EnumMember]
		LOOT_LIST_DROP_MULTI = 1,
		[EnumMember]
		LOOT_LIST_DROP_PERSONAL = 2,
		[EnumMember]
		LOOT_LIST_DROP_PERSONAL_PUSH = 3,
		[EnumMember]
		LOOT_LIST_DROP_PERSONAL_PUSH_FORCE_CLAIM = 4
	}
}
