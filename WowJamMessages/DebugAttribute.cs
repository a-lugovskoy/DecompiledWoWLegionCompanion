using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "DebugAttribute", Version = 28333852u), DataContract]
	public class DebugAttribute
	{
		[FlexJamMember(Name = "key", Type = FlexJamType.String), DataMember(Name = "key")]
		public string Key
		{
			get;
			set;
		}

		[FlexJamMember(Name = "value", Type = FlexJamType.Struct), DataMember(Name = "value")]
		public AttributeValue Value
		{
			get;
			set;
		}

		[FlexJamMember(Name = "param", Type = FlexJamType.Int32), DataMember(Name = "param")]
		public int Param
		{
			get;
			set;
		}
	}
}
