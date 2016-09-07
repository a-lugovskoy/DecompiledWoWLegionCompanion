using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[DataContract]
	public enum LOOT_LIST_ITEM_TYPE
	{
		[EnumMember]
		LOOT_LIST_ITEM = 0,
		[EnumMember]
		LOOT_LIST_CURRENCY = 1,
		[EnumMember]
		LOOT_LIST_TRACKING_QUEST = 2
	}
}
