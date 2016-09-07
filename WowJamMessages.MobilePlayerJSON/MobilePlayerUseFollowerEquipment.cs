using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4802, Name = "MobilePlayerUseFollowerEquipment", Version = 28333852u), DataContract]
	public class MobilePlayerUseFollowerEquipment
	{
		[FlexJamMember(Name = "garrFollowerTypeID", Type = FlexJamType.Int32), DataMember(Name = "garrFollowerTypeID")]
		public int GarrFollowerTypeID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "replaceAbilityID", Type = FlexJamType.Int32), DataMember(Name = "replaceAbilityID")]
		public int ReplaceAbilityID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrFollowerID", Type = FlexJamType.Int32), DataMember(Name = "garrFollowerID")]
		public int GarrFollowerID
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
	}
}
