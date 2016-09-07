using System;

namespace bgs.types
{
	public struct PartyListenerEvent
	{
		public enum AttributeChangeEvent_AttrType
		{
			ATTR_TYPE_NULL = 0,
			ATTR_TYPE_LONG = 1,
			ATTR_TYPE_STRING = 2,
			ATTR_TYPE_BLOB = 3
		}

		public PartyListenerEventType Type;

		public PartyId PartyId;

		public BnetGameAccountId SubjectMemberId;

		public BnetGameAccountId TargetMemberId;

		public uint UintData;

		public ulong UlongData;

		public string StringData;

		public string StringData2;

		public byte[] BlobData;

		public PartyError ToPartyError()
		{
			return new PartyError
			{
				IsOperationCallback = this.Type == PartyListenerEventType.OPERATION_CALLBACK,
				DebugContext = this.StringData,
				ErrorCode = (bgs.BattleNetErrors)this.UintData,
				Feature = (BnetFeature)(this.UlongData >> 32),
				FeatureEvent = (BnetFeatureEvent)((uint)(this.UlongData & (ulong)-1)),
				PartyId = this.PartyId,
				szPartyType = this.StringData2,
				StringData = this.StringData
			};
		}
	}
}
