using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonMissionReward", Version = 28333852u), DataContract]
	public class JamGarrisonMissionReward
	{
		[FlexJamMember(Name = "itemFileDataID", Type = FlexJamType.Int32), DataMember(Name = "itemFileDataID")]
		public int ItemFileDataID
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

		[FlexJamMember(Name = "currencyType", Type = FlexJamType.Int32), DataMember(Name = "currencyType")]
		public int CurrencyType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "followerXP", Type = FlexJamType.UInt32), DataMember(Name = "followerXP")]
		public uint FollowerXP
		{
			get;
			set;
		}

		[FlexJamMember(Name = "currencyQuantity", Type = FlexJamType.UInt32), DataMember(Name = "currencyQuantity")]
		public uint CurrencyQuantity
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemQuantity", Type = FlexJamType.UInt32), DataMember(Name = "itemQuantity")]
		public uint ItemQuantity
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrMssnBonusAbilityID", Type = FlexJamType.UInt32), DataMember(Name = "garrMssnBonusAbilityID")]
		public uint GarrMssnBonusAbilityID
		{
			get;
			set;
		}

		public JamGarrisonMissionReward()
		{
			this.ItemFileDataID = 0;
		}
	}
}
