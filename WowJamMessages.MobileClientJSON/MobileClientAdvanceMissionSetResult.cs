using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4843, Name = "MobileClientAdvanceMissionSetResult", Version = 28333852u), DataContract]
	public class MobileClientAdvanceMissionSetResult
	{
		[FlexJamMember(Name = "missionSetID", Type = FlexJamType.Int32), DataMember(Name = "missionSetID")]
		public int MissionSetID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "success", Type = FlexJamType.Bool), DataMember(Name = "success")]
		public bool Success
		{
			get;
			set;
		}
	}
}
