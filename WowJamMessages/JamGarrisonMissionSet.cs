using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonMissionSet", Version = 28333852u), DataContract]
	public class JamGarrisonMissionSet
	{
		[FlexJamMember(Name = "garrMissionSetID", Type = FlexJamType.Int32), DataMember(Name = "garrMissionSetID")]
		public int GarrMissionSetID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "lastUpdateTime", Type = FlexJamType.Int32), DataMember(Name = "lastUpdateTime")]
		public int LastUpdateTime
		{
			get;
			set;
		}
	}
}
