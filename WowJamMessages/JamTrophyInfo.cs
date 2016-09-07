using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamTrophyInfo", Version = 28333852u), DataContract]
	public class JamTrophyInfo
	{
		[FlexJamMember(Name = "canUseReason", Type = FlexJamType.Int32), DataMember(Name = "canUseReason")]
		public int CanUseReason
		{
			get;
			set;
		}

		[FlexJamMember(Name = "canUseData", Type = FlexJamType.Int32), DataMember(Name = "canUseData")]
		public int CanUseData
		{
			get;
			set;
		}

		[FlexJamMember(Name = "trophyID", Type = FlexJamType.Int32), DataMember(Name = "trophyID")]
		public int TrophyID
		{
			get;
			set;
		}
	}
}
