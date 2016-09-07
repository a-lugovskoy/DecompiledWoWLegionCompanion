using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileFollowerEquipment", Version = 28333852u), DataContract]
	public class MobileFollowerEquipment
	{
		[FlexJamMember(Name = "spellID", Type = FlexJamType.Int32), DataMember(Name = "spellID")]
		public int SpellID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrAbilityID", Type = FlexJamType.Int32), DataMember(Name = "garrAbilityID")]
		public int GarrAbilityID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemID", Type = FlexJamType.Int32), DataMember(Name = "itemID")]
		public int ItemID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "quantity", Type = FlexJamType.Int32), DataMember(Name = "quantity")]
		public int Quantity
		{
			get;
			set;
		}
	}
}
