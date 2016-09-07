using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[DataContract]
	public enum REALM_POPULATION_STATE
	{
		[EnumMember]
		REALM_POPULATION_STATE_OFFLINE = 0,
		[EnumMember]
		REALM_POPULATION_STATE_LOW = 1,
		[EnumMember]
		REALM_POPULATION_STATE_MEDIUM = 2,
		[EnumMember]
		REALM_POPULATION_STATE_HIGH = 3,
		[EnumMember]
		REALM_POPULATION_STATE_NEW = 4,
		[EnumMember]
		REALM_POPULATION_STATE_RECOMMENDED = 5,
		[EnumMember]
		REALM_POPULATION_STATE_FULL = 6,
		[EnumMember]
		REALM_POPULATION_STATE_LOCKED = 7
	}
}
