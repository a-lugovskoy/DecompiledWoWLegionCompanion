using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4784, Name = "MobilePlayerClaimMissionBonus", Version = 28333852u), DataContract]
	public class MobilePlayerClaimMissionBonus
	{
		[FlexJamMember(Name = "garrMissionID", Type = FlexJamType.Int32), DataMember(Name = "garrMissionID")]
		public int GarrMissionID
		{
			get;
			set;
		}
	}
}
