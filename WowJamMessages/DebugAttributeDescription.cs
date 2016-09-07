using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "DebugAttributeDescription", Version = 28333852u), DataContract]
	public class DebugAttributeDescription
	{
		[FlexJamMember(Name = "key", Type = FlexJamType.String), DataMember(Name = "key")]
		public string Key
		{
			get;
			set;
		}

		[FlexJamMember(Name = "descriptionData", Type = FlexJamType.String), DataMember(Name = "descriptionData")]
		public string DescriptionData
		{
			get;
			set;
		}

		[FlexJamMember(Name = "type", Type = FlexJamType.Int32), DataMember(Name = "type")]
		public int Type
		{
			get;
			set;
		}

		[FlexJamMember(Name = "flags", Type = FlexJamType.Int32), DataMember(Name = "flags")]
		public int Flags
		{
			get;
			set;
		}
	}
}
