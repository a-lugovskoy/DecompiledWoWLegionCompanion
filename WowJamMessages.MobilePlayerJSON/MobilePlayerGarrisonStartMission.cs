using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4782, Name = "MobilePlayerGarrisonStartMission", Version = 28333852u), DataContract]
	public class MobilePlayerGarrisonStartMission
	{
		[FlexJamMember(Name = "garrMissionID", Type = FlexJamType.Int32), DataMember(Name = "garrMissionID")]
		public int GarrMissionID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "followerDBIDs", Type = FlexJamType.UInt64), DataMember(Name = "followerDBIDs")]
		public ulong[] FollowerDBIDs
		{
			get;
			set;
		}
	}
}
