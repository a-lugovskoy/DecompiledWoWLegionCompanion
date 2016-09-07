using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "AITriggerActionSetDebugInfo", Version = 28333852u), DataContract]
	public class AITriggerActionSetDebugInfo
	{
		[FlexJamMember(Name = "aiTriggerActionSetID", Type = FlexJamType.Int32), DataMember(Name = "aiTriggerActionSetID")]
		public int AiTriggerActionSetID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "name", Type = FlexJamType.String), DataMember(Name = "name")]
		public string Name
		{
			get;
			set;
		}
	}
}
