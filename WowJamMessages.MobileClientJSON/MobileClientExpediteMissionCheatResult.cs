using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4842, Name = "MobileClientExpediteMissionCheatResult", Version = 28333852u), DataContract]
	public class MobileClientExpediteMissionCheatResult
	{
		[FlexJamMember(Name = "result", Type = FlexJamType.Int32), DataMember(Name = "result")]
		public int Result
		{
			get;
			set;
		}

		[FlexJamMember(Name = "missionRecID", Type = FlexJamType.Int32), DataMember(Name = "missionRecID")]
		public int MissionRecID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "mission", Type = FlexJamType.Struct), DataMember(Name = "mission")]
		public JamGarrisonMobileMission Mission
		{
			get;
			set;
		}
	}
}
