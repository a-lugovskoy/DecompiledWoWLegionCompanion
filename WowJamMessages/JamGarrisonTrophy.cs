using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonTrophy", Version = 28333852u), DataContract]
	public class JamGarrisonTrophy
	{
		[FlexJamMember(Name = "trophyInstanceID", Type = FlexJamType.Int32), DataMember(Name = "trophyInstanceID")]
		public int TrophyInstanceID
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
