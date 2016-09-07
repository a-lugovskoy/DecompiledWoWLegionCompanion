using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[DataContract]
	public enum AttributeValueType
	{
		[EnumMember]
		AVT_INT = 0,
		[EnumMember]
		AVT_FLOAT = 1,
		[EnumMember]
		AVT_STRING = 2,
		[EnumMember]
		AVT_GUID = 3,
		[EnumMember]
		AVT_VECTOR3 = 4
	}
}
