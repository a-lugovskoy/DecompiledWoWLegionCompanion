using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4855, Name = "MobileClientEvaluateMissionResult", Version = 28333852u), DataContract]
	public class MobileClientEvaluateMissionResult
	{
		[FlexJamMember(Name = "result", Type = FlexJamType.Int32), DataMember(Name = "result")]
		public int Result
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

		[FlexJamMember(Name = "garrMissionID", Type = FlexJamType.Int32), DataMember(Name = "garrMissionID")]
		public int GarrMissionID
		{
			get;
			set;
		}
	}
}
