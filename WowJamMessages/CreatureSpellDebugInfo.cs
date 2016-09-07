using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "CreatureSpellDebugInfo", Version = 28333852u), DataContract]
	public class CreatureSpellDebugInfo
	{
		[FlexJamMember(Name = "spellID", Type = FlexJamType.Int32), DataMember(Name = "spellID")]
		public int SpellID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "availability", Type = FlexJamType.Int32), DataMember(Name = "availability")]
		public int Availability
		{
			get;
			set;
		}

		[FlexJamMember(Name = "initialDelayMax", Type = FlexJamType.Int32), DataMember(Name = "initialDelayMax")]
		public int InitialDelayMax
		{
			get;
			set;
		}

		[FlexJamMember(Name = "spellName", Type = FlexJamType.String), DataMember(Name = "spellName")]
		public string SpellName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "repeatFrequencyMin", Type = FlexJamType.Int32), DataMember(Name = "repeatFrequencyMin")]
		public int RepeatFrequencyMin
		{
			get;
			set;
		}

		[FlexJamMember(Name = "priority", Type = FlexJamType.Int32), DataMember(Name = "priority")]
		public int Priority
		{
			get;
			set;
		}

		[FlexJamMember(Name = "initialDelayMin", Type = FlexJamType.Int32), DataMember(Name = "initialDelayMin")]
		public int InitialDelayMin
		{
			get;
			set;
		}

		[FlexJamMember(Name = "repeatFrequencyMax", Type = FlexJamType.Int32), DataMember(Name = "repeatFrequencyMax")]
		public int RepeatFrequencyMax
		{
			get;
			set;
		}
	}
}
