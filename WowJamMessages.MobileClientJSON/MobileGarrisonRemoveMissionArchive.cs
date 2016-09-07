using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4839, Name = "MobileGarrisonRemoveMissionArchive", Version = 28333852u), DataContract]
	public class MobileGarrisonRemoveMissionArchive
	{
		[FlexJamMember(Name = "result", Type = FlexJamType.Int32), DataMember(Name = "result")]
		public int Result
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

		[FlexJamMember(Name = "missionID", Type = FlexJamType.Int32), DataMember(Name = "missionID")]
		public int MissionID
		{
			get;
			set;
		}
	}
}
