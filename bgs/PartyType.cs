using System;
using System.ComponentModel;

namespace bgs
{
	public enum PartyType
	{
		[Description("default")]
		DEFAULT = 0,
		[Description("FriendlyGame")]
		FRIENDLY_CHALLENGE = 1,
		[Description("SpectatorParty")]
		SPECTATOR_PARTY = 2
	}
}
