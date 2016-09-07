using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "AITriggerActionDebugInfo", Version = 28333852u), DataContract]
	public class AITriggerActionDebugInfo
	{
		[FlexJamMember(Name = "repeatCount", Type = FlexJamType.Int32), DataMember(Name = "repeatCount")]
		public int RepeatCount
		{
			get;
			set;
		}

		[FlexJamMember(Name = "triggerDescription", Type = FlexJamType.String), DataMember(Name = "triggerDescription")]
		public string TriggerDescription
		{
			get;
			set;
		}

		[FlexJamMember(Name = "aiGroupActionSetID", Type = FlexJamType.Int32), DataMember(Name = "aiGroupActionSetID")]
		public int AiGroupActionSetID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "note", Type = FlexJamType.String), DataMember(Name = "note")]
		public string Note
		{
			get;
			set;
		}

		[FlexJamMember(Name = "typeName", Type = FlexJamType.String), DataMember(Name = "typeName")]
		public string TypeName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "triggerTime", Type = FlexJamType.UInt32), DataMember(Name = "triggerTime")]
		public uint TriggerTime
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

		[FlexJamMember(Name = "triggerData", Type = FlexJamType.Int32), DataMember(Name = "triggerData")]
		public int TriggerData
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "param", Type = FlexJamType.Int32), DataMember(Name = "param")]
		public int[] Param
		{
			get;
			set;
		}

		[FlexJamMember(Name = "aiGroupActionSetName", Type = FlexJamType.String), DataMember(Name = "aiGroupActionSetName")]
		public string AiGroupActionSetName
		{
			get;
			set;
		}

		public AITriggerActionDebugInfo()
		{
			this.TypeName = string.Empty;
			this.Param = new int[2];
			this.TriggerDescription = string.Empty;
			this.Note = string.Empty;
			this.AiGroupActionSetName = string.Empty;
		}
	}
}
