using System;
using System.Runtime.InteropServices;

namespace bgs.types
{
	public struct ChallengeInfo
	{
		public ulong challengeId;

		[MarshalAs(3)]
		public bool isRetry;

		public int type;
	}
}
