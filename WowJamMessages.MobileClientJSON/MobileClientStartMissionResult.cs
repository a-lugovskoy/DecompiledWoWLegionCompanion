using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4835, Name = "MobileClientStartMissionResult", Version = 28333852u), DataContract]
	public class MobileClientStartMissionResult
	{
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

		[FlexJamMember(Name = "newDailyMissionCounter", Type = FlexJamType.UInt16), DataMember(Name = "newDailyMissionCounter")]
		public ushort NewDailyMissionCounter
		{
			get;
			set;
		}
	}
}
