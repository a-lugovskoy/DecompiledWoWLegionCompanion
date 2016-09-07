using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayPurchase", Version = 28333852u), DataContract]
	public class JamBattlePayPurchase
	{
		[FlexJamMember(Name = "purchaseID", Type = FlexJamType.UInt64), DataMember(Name = "purchaseID")]
		public ulong PurchaseID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "status", Type = FlexJamType.UInt32), DataMember(Name = "status")]
		public uint Status
		{
			get;
			set;
		}

		[FlexJamMember(Name = "resultCode", Type = FlexJamType.UInt32), DataMember(Name = "resultCode")]
		public uint ResultCode
		{
			get;
			set;
		}

		[FlexJamMember(Name = "productID", Type = FlexJamType.UInt32), DataMember(Name = "productID")]
		public uint ProductID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "walletName", Type = FlexJamType.String), DataMember(Name = "walletName")]
		public string WalletName
		{
			get;
			set;
		}
	}
}
