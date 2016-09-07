using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "ScriptTableValueDebugInfo", Version = 28333852u), DataContract]
	public class ScriptTableValueDebugInfo
	{
		[FlexJamMember(Name = "keyName", Type = FlexJamType.String), DataMember(Name = "keyName")]
		public string KeyName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "valueName", Type = FlexJamType.String), DataMember(Name = "valueName")]
		public string ValueName
		{
			get;
			set;
		}
	}
}
