using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamPartyMemberAuraState", Version = 28333852u), DataContract]
	public class JamPartyMemberAuraState
	{
		[FlexJamMember(Name = "activeFlags", Type = FlexJamType.UInt32), DataMember(Name = "activeFlags")]
		public uint ActiveFlags
		{
			get;
			set;
		}

		[FlexJamMember(Name = "aura", Type = FlexJamType.Int32), DataMember(Name = "aura")]
		public int Aura
		{
			get;
			set;
		}

		[FlexJamMember(Name = "flags", Type = FlexJamType.UInt8), DataMember(Name = "flags")]
		public byte Flags
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "points", Type = FlexJamType.Float), DataMember(Name = "points")]
		public float[] Points
		{
			get;
			set;
		}
	}
}
