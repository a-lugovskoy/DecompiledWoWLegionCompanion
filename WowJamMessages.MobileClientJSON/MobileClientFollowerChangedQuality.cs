using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4861, Name = "MobileClientFollowerChangedQuality", Version = 28333852u), DataContract]
	public class MobileClientFollowerChangedQuality
	{
		[FlexJamMember(Name = "oldFollower", Type = FlexJamType.Struct), DataMember(Name = "oldFollower")]
		public JamGarrisonFollower OldFollower
		{
			get;
			set;
		}

		[FlexJamMember(Name = "follower", Type = FlexJamType.Struct), DataMember(Name = "follower")]
		public JamGarrisonFollower Follower
		{
			get;
			set;
		}
	}
}
