using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4856, Name = "MobileClientSetMissionDurationCheatResult", Version = 28333852u), DataContract]
	public class MobileClientSetMissionDurationCheatResult
	{
		[FlexJamMember(Name = "success", Type = FlexJamType.Bool), DataMember(Name = "success")]
		public bool Success
		{
			get;
			set;
		}
	}
}
