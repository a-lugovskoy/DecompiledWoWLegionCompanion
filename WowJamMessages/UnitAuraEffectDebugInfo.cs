using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "UnitAuraEffectDebugInfo", Version = 28333852u), DataContract]
	public class UnitAuraEffectDebugInfo
	{
		[FlexJamMember(Name = "active", Type = FlexJamType.Bool), DataMember(Name = "active")]
		public bool Active
		{
			get;
			set;
		}

		[FlexJamMember(Name = "effectIndex", Type = FlexJamType.Int32), DataMember(Name = "effectIndex")]
		public int EffectIndex
		{
			get;
			set;
		}

		[FlexJamMember(Name = "amount", Type = FlexJamType.Float), DataMember(Name = "amount")]
		public float Amount
		{
			get;
			set;
		}
	}
}
