using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4832, Name = "MobileClientConnectResult", Version = 28333852u), DataContract]
	public class MobileClientConnectResult
	{
		[FlexJamMember(Name = "result", Type = FlexJamType.Enum), DataMember(Name = "result")]
		public MOBILE_CONNECT_RESULT Result
		{
			get;
			set;
		}

		[FlexJamMember(Name = "version", Type = FlexJamType.Int32), DataMember(Name = "version")]
		public int Version
		{
			get;
			set;
		}

		public MobileClientConnectResult()
		{
			this.Version = 0;
		}
	}
}
