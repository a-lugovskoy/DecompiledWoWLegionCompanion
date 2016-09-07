using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "DebugEvent", Version = 28333852u), DataContract]
	public class DebugEvent
	{
		[FlexJamMember(Name = "eventName", Type = FlexJamType.String), DataMember(Name = "eventName")]
		public string EventName
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string[] Guid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "messageText", Type = FlexJamType.String), DataMember(Name = "messageText")]
		public string MessageText
		{
			get;
			set;
		}

		[FlexJamMember(Name = "systemNameHash", Type = FlexJamType.Int32), DataMember(Name = "systemNameHash")]
		public int SystemNameHash
		{
			get;
			set;
		}

		[FlexJamMember(Name = "eventTime", Type = FlexJamType.Int32), DataMember(Name = "eventTime")]
		public int EventTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "systemName", Type = FlexJamType.String), DataMember(Name = "systemName")]
		public string SystemName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "messageTextHash", Type = FlexJamType.Int32), DataMember(Name = "messageTextHash")]
		public int MessageTextHash
		{
			get;
			set;
		}

		[FlexJamMember(Name = "eventNameHash", Type = FlexJamType.Int32), DataMember(Name = "eventNameHash")]
		public int EventNameHash
		{
			get;
			set;
		}
	}
}
