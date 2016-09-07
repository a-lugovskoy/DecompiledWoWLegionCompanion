using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4788, Name = "MobilePlayerGarrisonAdvanceMissionSet", Version = 28333852u), DataContract]
	public class MobilePlayerGarrisonAdvanceMissionSet
	{
		[FlexJamMember(Name = "missionSetID", Type = FlexJamType.Int32), DataMember(Name = "missionSetID")]
		public int MissionSetID
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
