using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonMissionFollowerInfo", Version = 28333852u), DataContract]
	public class JamGarrisonMissionFollowerInfo
	{
		[FlexJamMember(Name = "followerDBID", Type = FlexJamType.UInt64), DataMember(Name = "followerDBID")]
		public ulong FollowerDBID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "missionCompleteState", Type = FlexJamType.UInt32), DataMember(Name = "missionCompleteState")]
		public uint MissionCompleteState
		{
			get;
			set;
		}
	}
}
