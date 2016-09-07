using System;

namespace WowStatConstants
{
	public enum DisconnectReason
	{
		None = 0,
		ConnectionLost = 1,
		TimeoutContactingServer = 2,
		AppVersionOld = 3,
		AppVersionNew = 4,
		Generic = 5,
		CharacterInWorld = 6,
		CantEnterWorld = 7,
		LoginDisabled = 8,
		TrialNotAllowed = 9,
		ConsumptionTimeNotAllowed = 10,
		PingTimeout = 11
	}
}
