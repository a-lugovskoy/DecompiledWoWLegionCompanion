using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonMobileMission", Version = 28333852u), DataContract]
	public class JamGarrisonMobileMission
	{
		[FlexJamMember(Name = "offerTime", Type = FlexJamType.Int64), DataMember(Name = "offerTime")]
		public long OfferTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "travelDuration", Type = FlexJamType.Int64), DataMember(Name = "travelDuration")]
		public long TravelDuration
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

		[FlexJamMember(Name = "missionState", Type = FlexJamType.Int32), DataMember(Name = "missionState")]
		public int MissionState
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "encounter", Type = FlexJamType.Struct), DataMember(Name = "encounter")]
		public JamGarrisonEncounter[] Encounter
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "reward", Type = FlexJamType.Struct), DataMember(Name = "reward")]
		public JamGarrisonMissionReward[] Reward
		{
			get;
			set;
		}

		[FlexJamMember(Name = "startTime", Type = FlexJamType.Int64), DataMember(Name = "startTime")]
		public long StartTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "dbID", Type = FlexJamType.UInt64), DataMember(Name = "dbID")]
		public ulong DbID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "offerDuration", Type = FlexJamType.Int64), DataMember(Name = "offerDuration")]
		public long OfferDuration
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "overmaxReward", Type = FlexJamType.Struct), DataMember(Name = "overmaxReward")]
		public JamGarrisonMissionReward[] OvermaxReward
		{
			get;
			set;
		}

		[FlexJamMember(Name = "missionDuration", Type = FlexJamType.Int64), DataMember(Name = "missionDuration")]
		public long MissionDuration
		{
			get;
			set;
		}
	}
}
