using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "PhaseShiftDataVersion1", Version = 28333852u), DataContract]
	public class PhaseShiftDataVersion1
	{
		[FlexJamMember(Name = "phaseShiftFlags", Type = FlexJamType.UInt32), DataMember(Name = "phaseShiftFlags")]
		public uint PhaseShiftFlags
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

		[FlexJamMember(ArrayDimensions = 1, Name = "phaseID", Type = FlexJamType.UInt16), DataMember(Name = "phaseID")]
		public ushort[] PhaseID
		{
			get;
			set;
		}
	}
}
