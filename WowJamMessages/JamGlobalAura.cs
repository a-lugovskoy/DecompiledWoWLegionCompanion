using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGlobalAura", Version = 28333852u), DataContract]
	public class JamGlobalAura
	{
		[FlexJamMember(Name = "spellID", Type = FlexJamType.Int32), DataMember(Name = "spellID")]
		public int SpellID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "playerConditionID", Type = FlexJamType.Int32), DataMember(Name = "playerConditionID")]
		public int PlayerConditionID
		{
			get;
			set;
		}
	}
}
