using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4864, Name = "MobileClientWorldQuestBountiesResult", Version = 28333852u), DataContract]
	public class MobileClientWorldQuestBountiesResult
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "bounty", Type = FlexJamType.Struct), DataMember(Name = "bounty")]
		public MobileWorldQuestBounty[] Bounty
		{
			get;
			set;
		}

		[FlexJamMember(Name = "visible", Type = FlexJamType.Bool), DataMember(Name = "visible")]
		public bool Visible
		{
			get;
			set;
		}

		[FlexJamMember(Name = "lockedQuestID", Type = FlexJamType.Int32), DataMember(Name = "lockedQuestID")]
		public int LockedQuestID
		{
			get;
			set;
		}
	}
}
