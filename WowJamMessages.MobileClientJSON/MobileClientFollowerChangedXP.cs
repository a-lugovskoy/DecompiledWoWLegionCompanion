using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4841, Name = "MobileClientFollowerChangedXP", Version = 28333852u), DataContract]
	public class MobileClientFollowerChangedXP
	{
		[FlexJamMember(Name = "follower", Type = FlexJamType.Struct), DataMember(Name = "follower")]
		public JamGarrisonFollower Follower
		{
			get;
			set;
		}

		[FlexJamMember(Name = "xpChange", Type = FlexJamType.Int32), DataMember(Name = "xpChange")]
		public int XpChange
		{
			get;
			set;
		}

		[FlexJamMember(Name = "source", Type = FlexJamType.Int32), DataMember(Name = "source")]
		public int Source
		{
			get;
			set;
		}

		[FlexJamMember(Name = "oldFollower", Type = FlexJamType.Struct), DataMember(Name = "oldFollower")]
		public JamGarrisonFollower OldFollower
		{
			get;
			set;
		}
	}
}
