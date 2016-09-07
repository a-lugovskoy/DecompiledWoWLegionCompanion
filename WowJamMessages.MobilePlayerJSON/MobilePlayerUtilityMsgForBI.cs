using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4809, Name = "MobilePlayerUtilityMsgForBI", Version = 28333852u), DataContract]
	public class MobilePlayerUtilityMsgForBI
	{
		[FlexJamMember(Name = "msgType", Type = FlexJamType.Int32), DataMember(Name = "msgType")]
		public int MsgType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "data4", Type = FlexJamType.Int32), DataMember(Name = "data4")]
		public int Data4
		{
			get;
			set;
		}

		[FlexJamMember(Name = "data3", Type = FlexJamType.Int32), DataMember(Name = "data3")]
		public int Data3
		{
			get;
			set;
		}

		[FlexJamMember(Name = "data2", Type = FlexJamType.Int32), DataMember(Name = "data2")]
		public int Data2
		{
			get;
			set;
		}

		[FlexJamMember(Name = "data1", Type = FlexJamType.Int32), DataMember(Name = "data1")]
		public int Data1
		{
			get;
			set;
		}
	}
}
