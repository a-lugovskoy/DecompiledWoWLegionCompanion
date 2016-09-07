using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4837, Name = "MobileClientClaimMissionBonusResult", Version = 28333852u), DataContract]
	public class MobileClientClaimMissionBonusResult
	{
		[FlexJamMember(Name = "awardOvermax", Type = FlexJamType.Bool), DataMember(Name = "awardOvermax")]
		public bool AwardOvermax
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrMissionID", Type = FlexJamType.Int32), DataMember(Name = "garrMissionID")]
		public int GarrMissionID
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

		[FlexJamMember(Name = "mission", Type = FlexJamType.Struct), DataMember(Name = "mission")]
		public JamGarrisonMobileMission Mission
		{
			get;
			set;
		}
	}
}
