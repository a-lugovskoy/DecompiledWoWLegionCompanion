using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonMissionBonusAbility", Version = 28333852u), DataContract]
	public class JamGarrisonMissionBonusAbility
	{
		[FlexJamMember(Name = "garrMssnBonusAbilityID", Type = FlexJamType.Int32), DataMember(Name = "garrMssnBonusAbilityID")]
		public int GarrMssnBonusAbilityID
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
	}
}
