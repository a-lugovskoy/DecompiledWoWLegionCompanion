using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "PhaseShiftData", Version = 28333852u), DataContract]
	public class PhaseShiftData
	{
		[FlexJamMember(Name = "phaseShiftFlags", Type = FlexJamType.UInt32), DataMember(Name = "phaseShiftFlags")]
		public uint PhaseShiftFlags
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "phases", Type = FlexJamType.Struct), DataMember(Name = "phases")]
		public PhaseShiftDataPhase[] Phases
		{
			get;
			set;
		}

		[FlexJamMember(Name = "personalGUID", Type = FlexJamType.WowGuid), DataMember(Name = "personalGUID")]
		public string PersonalGUID
		{
			get;
			set;
		}
	}
}
