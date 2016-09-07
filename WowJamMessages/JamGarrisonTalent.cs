using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonTalent", Version = 28333852u), DataContract]
	public class JamGarrisonTalent
	{
		[FlexJamMember(Name = "flags", Type = FlexJamType.Int32), DataMember(Name = "flags")]
		public int Flags
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrTalentID", Type = FlexJamType.Int32), DataMember(Name = "garrTalentID")]
		public int GarrTalentID
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
