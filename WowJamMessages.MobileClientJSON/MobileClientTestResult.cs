using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4831, Name = "MobileClientTestResult", Version = 28333852u), DataContract]
	public class MobileClientTestResult
	{
		[FlexJamMember(Name = "result", Type = FlexJamType.String), DataMember(Name = "result")]
		public string Result
		{
			get;
			set;
		}
	}
}
