using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4838, Name = "MobileClientMissionAdded", Version = 28333852u), DataContract]
	public class MobileClientMissionAdded
	{
		[FlexJamMember(Name = "mission", Type = FlexJamType.Struct), DataMember(Name = "mission")]
		public JamGarrisonMobileMission Mission
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

		[FlexJamMember(Name = "missionSource", Type = FlexJamType.UInt8), DataMember(Name = "missionSource")]
		public byte MissionSource
		{
			get;
			set;
		}

		[FlexJamMember(Name = "canStartMission", Type = FlexJamType.Bool), DataMember(Name = "canStartMission")]
		public bool CanStartMission
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrTypeID", Type = FlexJamType.Int32), DataMember(Name = "garrTypeID")]
		public int GarrTypeID
		{
			get;
			set;
		}
	}
}
