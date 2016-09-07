using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "AttributeValue", Version = 28333852u), DataContract]
	public class AttributeValue
	{
		[FlexJamMember(Name = "intValue", Type = FlexJamType.Int32), DataMember(Name = "intValue")]
		public int IntValue
		{
			get;
			set;
		}

		[FlexJamMember(Name = "vector3Value", Type = FlexJamType.Struct), DataMember(Name = "vector3Value")]
		public Vector3 Vector3Value
		{
			get;
			set;
		}

		[FlexJamMember(Name = "type", Type = FlexJamType.Enum), DataMember(Name = "type")]
		public AttributeValueType Type
		{
			get;
			set;
		}

		[FlexJamMember(Name = "floatValue", Type = FlexJamType.Float), DataMember(Name = "floatValue")]
		public float FloatValue
		{
			get;
			set;
		}

		[FlexJamMember(Name = "stringValue", Type = FlexJamType.String), DataMember(Name = "stringValue")]
		public string StringValue
		{
			get;
			set;
		}

		[FlexJamMember(Name = "guidValue", Type = FlexJamType.WowGuid), DataMember(Name = "guidValue")]
		public string GuidValue
		{
			get;
			set;
		}

		public AttributeValue()
		{
			this.IntValue = 0;
			this.FloatValue = 0f;
			this.StringValue = string.Empty;
			this.GuidValue = "0000000000000000";
		}
	}
}
