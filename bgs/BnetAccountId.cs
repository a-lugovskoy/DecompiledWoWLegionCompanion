using bgs.types;
using System;

namespace bgs
{
	public class BnetAccountId : BnetEntityId
	{
		public static BnetAccountId CreateFromEntityId(EntityId src)
		{
			BnetAccountId bnetAccountId = new BnetAccountId();
			bnetAccountId.CopyFrom(src);
			return bnetAccountId;
		}

		public static BnetAccountId CreateFromBnetEntityId(BnetEntityId src)
		{
			BnetAccountId bnetAccountId = new BnetAccountId();
			bnetAccountId.CopyFrom(src);
			return bnetAccountId;
		}

		public BnetAccountId Clone()
		{
			return (BnetAccountId)base.Clone();
		}
	}
}
