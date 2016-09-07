using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4796, Name = "MobilePlayerEvaluateMission", Version = 28333852u), DataContract]
	public class MobilePlayerEvaluateMission
	{
		[FlexJamMember(Name = "garrMissionID", Type = FlexJamType.Int32), DataMember(Name = "garrMissionID")]
		public int GarrMissionID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "garrFollowerID", Type = FlexJamType.Int32), DataMember(Name = "garrFollowerID")]
		public int[] GarrFollowerID
		{
			get;
			set;
		}
	}
}
