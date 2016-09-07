using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonMission", Version = 28333852u), DataContract]
	public class JamGarrisonMission
	{
		[FlexJamMember(Name = "offerTime", Type = FlexJamType.Int32), DataMember(Name = "offerTime")]
		public int OfferTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "successChance", Type = FlexJamType.Int32), DataMember(Name = "successChance")]
		public int SuccessChance
		{
			get;
			set;
		}

		[FlexJamMember(Name = "travelDuration", Type = FlexJamType.Int32), DataMember(Name = "travelDuration")]
		public int TravelDuration
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

		[FlexJamMember(Name = "flags", Type = FlexJamType.UInt32), DataMember(Name = "flags")]
		public uint Flags
		{
			get;
			set;
		}

		[FlexJamMember(Name = "startTime", Type = FlexJamType.Int32), DataMember(Name = "startTime")]
		public int StartTime
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

		[FlexJamMember(Name = "offerDuration", Type = FlexJamType.Int32), DataMember(Name = "offerDuration")]
		public int OfferDuration
		{
			get;
			set;
		}

		[FlexJamMember(Name = "missionDuration", Type = FlexJamType.Int32), DataMember(Name = "missionDuration")]
		public int MissionDuration
		{
			get;
			set;
		}

		public JamGarrisonMission()
		{
			this.Flags = 0u;
		}
	}
}
