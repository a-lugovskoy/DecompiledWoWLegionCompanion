using bgs.types;
using bnet.protocol;
using System;

namespace bgs
{
	public class BnetGameAccountId : BnetEntityId
	{
		public static BnetGameAccountId CreateFromEntityId(bgs.types.EntityId src)
		{
			BnetGameAccountId bnetGameAccountId = new BnetGameAccountId();
			bnetGameAccountId.CopyFrom(src);
			return bnetGameAccountId;
		}

		public static BnetGameAccountId CreateFromProtocol(bnet.protocol.EntityId src)
		{
			BnetGameAccountId bnetGameAccountId = new BnetGameAccountId();
			bnetGameAccountId.SetLo(src.Low);
			bnetGameAccountId.SetHi(src.High);
			return bnetGameAccountId;
		}

		public BnetGameAccountId Clone()
		{
			return (BnetGameAccountId)base.Clone();
		}
	}
}
