using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileItemBonusStat", Version = 28333852u), DataContract]
	public class MobileItemBonusStat
	{
		[FlexJamMember(Name = "statID", Type = FlexJamType.Int32), DataMember(Name = "statID")]
		public int StatID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "bonusAmount", Type = FlexJamType.Int32), DataMember(Name = "bonusAmount")]
		public int BonusAmount
		{
			get;
			set;
		}

		[FlexJamMember(Name = "color", Type = FlexJamType.Enum), DataMember(Name = "color")]
		public MobileStatColor Color
		{
			get;
			set;
		}
	}
}
