using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "SpawnEventDebugInfo", Version = 28333852u), DataContract]
	public class SpawnEventDebugInfo
	{
		[FlexJamMember(Name = "eventID", Type = FlexJamType.Int32), DataMember(Name = "eventID")]
		public int EventID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "eventName", Type = FlexJamType.String), DataMember(Name = "eventName")]
		public string EventName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "entryNum", Type = FlexJamType.Int32), DataMember(Name = "entryNum")]
		public int EntryNum
		{
			get;
			set;
		}

		[FlexJamMember(Name = "eventPercent", Type = FlexJamType.Int32), DataMember(Name = "eventPercent")]
		public int EventPercent
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

		[FlexJamMember(Name = "aiGroupActionSetName", Type = FlexJamType.String), DataMember(Name = "aiGroupActionSetName")]
		public string AiGroupActionSetName
		{
			get;
			set;
		}

		public SpawnEventDebugInfo()
		{
			this.EventName = string.Empty;
			this.AiGroupActionSetName = string.Empty;
		}
	}
}
