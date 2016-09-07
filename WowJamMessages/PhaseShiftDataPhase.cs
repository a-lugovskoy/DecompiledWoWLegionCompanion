using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "PhaseShiftDataPhase", Version = 28333852u), DataContract]
	public class PhaseShiftDataPhase
	{
		[FlexJamMember(Name = "id", Type = FlexJamType.UInt16), DataMember(Name = "id")]
		public ushort Id
		{
			get;
			set;
		}

		[FlexJamMember(Name = "phaseFlags", Type = FlexJamType.UInt16), DataMember(Name = "phaseFlags")]
		public ushort PhaseFlags
		{
			get;
			set;
		}
	}
}
