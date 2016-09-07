using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "SystemDebugInfo", Version = 28333852u), DataContract]
	public class SystemDebugInfo
	{
		[FlexJamMember(Name = "name", Type = FlexJamType.String), DataMember(Name = "name")]
		public string Name
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "attributeDescriptions", Type = FlexJamType.Struct), DataMember(Name = "attributeDescriptions")]
		public DebugAttributeDescription[] AttributeDescriptions
		{
			get;
			set;
		}

		[FlexJamMember(Name = "updateTime", Type = FlexJamType.Int32), DataMember(Name = "updateTime")]
		public int UpdateTime
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "attributes", Type = FlexJamType.Struct), DataMember(Name = "attributes")]
		public DebugAttribute[] Attributes
		{
			get;
			set;
		}

		[FlexJamMember(Name = "requestParameter", Type = FlexJamType.String), DataMember(Name = "requestParameter")]
		public string RequestParameter
		{
			get;
			set;
		}

		public SystemDebugInfo()
		{
			this.RequestParameter = string.Empty;
		}
	}
}
