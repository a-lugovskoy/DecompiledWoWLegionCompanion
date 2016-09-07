using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayDistributionObject", Version = 28333852u), DataContract]
	public class JamBattlePayDistributionObject
	{
		[FlexJamMember(Name = "revoked", Type = FlexJamType.Bool), DataMember(Name = "revoked")]
		public bool Revoked
		{
			get;
			set;
		}

		[FlexJamMember(Name = "deliverableID", Type = FlexJamType.UInt32), DataMember(Name = "deliverableID")]
		public uint DeliverableID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "targetPlayer", Type = FlexJamType.WowGuid), DataMember(Name = "targetPlayer")]
		public string TargetPlayer
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "deliverable", Type = FlexJamType.Struct), DataMember(Name = "deliverable")]
		public JamBattlePayDeliverable[] Deliverable
		{
			get;
			set;
		}

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

		[FlexJamMember(Name = "targetNativeRealm", Type = FlexJamType.UInt32), DataMember(Name = "targetNativeRealm")]
		public uint TargetNativeRealm
		{
			get;
			set;
		}

		[FlexJamMember(Name = "distributionID", Type = FlexJamType.UInt64), DataMember(Name = "distributionID")]
		public ulong DistributionID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "targetVirtualRealm", Type = FlexJamType.UInt32), DataMember(Name = "targetVirtualRealm")]
		public uint TargetVirtualRealm
		{
			get;
			set;
		}
	}
}
