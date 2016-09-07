using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4836, Name = "MobileClientCompleteMissionResult", Version = 28333852u), DataContract]
	public class MobileClientCompleteMissionResult
	{
		[FlexJamMember(Name = "garrMissionID", Type = FlexJamType.Int32), DataMember(Name = "garrMissionID")]
		public int GarrMissionID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "missionSuccessChance", Type = FlexJamType.UInt8), DataMember(Name = "missionSuccessChance")]
		public byte MissionSuccessChance
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

		[FlexJamMember(Name = "bonusRollSucceeded", Type = FlexJamType.Bool), DataMember(Name = "bonusRollSucceeded")]
		public bool BonusRollSucceeded
		{
			get;
			set;
		}

		[FlexJamMember(Name = "result", Type = FlexJamType.Int32), DataMember(Name = "result")]
		public int Result
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "followerInfo", Type = FlexJamType.Struct), DataMember(Name = "followerInfo")]
		public JamGarrisonMissionFollowerInfo[] FollowerInfo
		{
			get;
			set;
		}
	}
}
