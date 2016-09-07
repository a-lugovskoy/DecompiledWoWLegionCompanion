using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "ObjectPhaseDebugInfo", Version = 28333852u), DataContract]
	public class ObjectPhaseDebugInfo
	{
		[FlexJamMember(Name = "phaseName", Type = FlexJamType.String), DataMember(Name = "phaseName")]
		public string PhaseName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "phaseID", Type = FlexJamType.Int32), DataMember(Name = "phaseID")]
		public int PhaseID
		{
			get;
			set;
		}
	}
}
