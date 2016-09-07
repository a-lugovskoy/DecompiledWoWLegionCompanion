using bnet.protocol;
using System;

namespace bgs.types
{
	public struct EntityId
	{
		public ulong hi;

		public ulong lo;

		public EntityId(EntityId copyFrom)
		{
			this.hi = copyFrom.hi;
			this.lo = copyFrom.lo;
		}

		public EntityId(bnet.protocol.EntityId protoEntityId)
		{
			this.hi = protoEntityId.High;
			this.lo = protoEntityId.Low;
		}

		public bnet.protocol.EntityId ToProtocol()
		{
			bnet.protocol.EntityId entityId = new bnet.protocol.EntityId();
			entityId.SetHigh(this.hi);
			entityId.SetLow(this.lo);
			return entityId;
		}
	}
}
