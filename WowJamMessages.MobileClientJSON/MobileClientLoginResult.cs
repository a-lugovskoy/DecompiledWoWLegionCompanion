using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4830, Name = "MobileClientLoginResult", Version = 28333852u), DataContract]
	public class MobileClientLoginResult
	{
		[FlexJamMember(Name = "success", Type = FlexJamType.Bool), DataMember(Name = "success")]
		public bool Success
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

		public MobileClientLoginResult()
		{
			this.Version = 0;
		}
	}
}
