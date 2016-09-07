using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4833, Name = "MobileClientGarrisonDataRequestResult", Version = 28333852u), DataContract]
	public class MobileClientGarrisonDataRequestResult
	{
		[FlexJamMember(Name = "orderhallResourcesCurrency", Type = FlexJamType.Int32), DataMember(Name = "orderhallResourcesCurrency")]
		public int OrderhallResourcesCurrency
		{
			get;
			set;
		}

		[FlexJamMember(Name = "pvpFaction", Type = FlexJamType.Int32), DataMember(Name = "pvpFaction")]
		public int PvpFaction
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "mission", Type = FlexJamType.Struct), DataMember(Name = "mission")]
		public JamGarrisonMobileMission[] Mission
		{
			get;
			set;
		}

		[FlexJamMember(Name = "oilCurrency", Type = FlexJamType.Int32), DataMember(Name = "oilCurrency")]
		public int OilCurrency
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "follower", Type = FlexJamType.Struct), DataMember(Name = "follower")]
		public JamGarrisonFollower[] Follower
		{
			get;
			set;
		}

		[FlexJamMember(Name = "characterClassID", Type = FlexJamType.Int32), DataMember(Name = "characterClassID")]
		public int CharacterClassID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "goldCurrency", Type = FlexJamType.Int32), DataMember(Name = "goldCurrency")]
		public int GoldCurrency
		{
			get;
			set;
		}

		[FlexJamMember(Name = "characterLevel", Type = FlexJamType.Int32), DataMember(Name = "characterLevel")]
		public int CharacterLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "serverTime", Type = FlexJamType.Int64), DataMember(Name = "serverTime")]
		public long ServerTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "dailyMissionCount", Type = FlexJamType.Int32), DataMember(Name = "dailyMissionCount")]
		public int DailyMissionCount
		{
			get;
			set;
		}

		[FlexJamMember(Name = "resourcesCurrency", Type = FlexJamType.Int32), DataMember(Name = "resourcesCurrency")]
		public int ResourcesCurrency
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "talent", Type = FlexJamType.Struct), DataMember(Name = "talent")]
		public JamGarrisonTalent[] Talent
		{
			get;
			set;
		}

		[FlexJamMember(Name = "characterName", Type = FlexJamType.String), DataMember(Name = "characterName")]
		public string CharacterName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrTypeID", Type = FlexJamType.Int32), DataMember(Name = "garrTypeID")]
		public int GarrTypeID
		{
			get;
			set;
		}
	}
}
