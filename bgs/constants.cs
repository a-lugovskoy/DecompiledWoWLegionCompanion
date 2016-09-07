using System;
using System.ComponentModel;

namespace bgs
{
	public class constants
	{
		public enum BNetState
		{
			BATTLE_NET_UNKNOWN = 0,
			BATTLE_NET_LOGGING_IN = 1,
			BATTLE_NET_TIMEOUT = 2,
			BATTLE_NET_LOGIN_FAILED = 3,
			BATTLE_NET_LOGGED_IN = 4
		}

		public enum BnetRegion
		{
			REGION_UNINITIALIZED = -1,
			REGION_UNKNOWN = 0,
			REGION_US = 1,
			REGION_EU = 2,
			REGION_KR = 3,
			REGION_TW = 4,
			REGION_CN = 5,
			REGION_LIVE_VERIFICATION = 40,
			REGION_PTR_LOC = 41,
			REGION_MSCHWEITZER_BN11 = 52,
			REGION_MSCHWEITZER_BN12 = 53,
			REGION_DEV = 60,
			REGION_PTR = 98
		}

		public enum MobileEnv
		{
			[Description("Development")]
			DEVELOPMENT = 0,
			[Description("Production")]
			PRODUCTION = 1
		}

		public enum RuntimeEnvironment
		{
			Mono = 0,
			MSDotNet = 1
		}

		public const ushort RouteToAnyUtil = 0;

		public const float ResubsribeAttemptDelaySeconds = 120f;
	}
}
