using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileWorldQuest", Version = 28333852u), DataContract]
	public class MobileWorldQuest
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "item", Type = FlexJamType.Struct), DataMember(Name = "item")]
		public MobileWorldQuestReward[] Item
		{
			get;
			set;
		}

		[FlexJamMember(Name = "endTime", Type = FlexJamType.Int32), DataMember(Name = "endTime")]
		public int EndTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "worldMapAreaID", Type = FlexJamType.Int32), DataMember(Name = "worldMapAreaID")]
		public int WorldMapAreaID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "experience", Type = FlexJamType.Int32), DataMember(Name = "experience")]
		public int Experience
		{
			get;
			set;
		}

		[FlexJamMember(Name = "startLocationMapID", Type = FlexJamType.Int32), DataMember(Name = "startLocationMapID")]
		public int StartLocationMapID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "questID", Type = FlexJamType.Int32), DataMember(Name = "questID")]
		public int QuestID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "questInfoID", Type = FlexJamType.Int32), DataMember(Name = "questInfoID")]
		public int QuestInfoID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "money", Type = FlexJamType.Int32), DataMember(Name = "money")]
		public int Money
		{
			get;
			set;
		}

		[FlexJamMember(Name = "startTime", Type = FlexJamType.Int32), DataMember(Name = "startTime")]
		public int StartTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "startLocationY", Type = FlexJamType.Int32), DataMember(Name = "startLocationY")]
		public int StartLocationY
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "currency", Type = FlexJamType.Struct), DataMember(Name = "currency")]
		public MobileWorldQuestReward[] Currency
		{
			get;
			set;
		}

		[FlexJamMember(Name = "startLocationX", Type = FlexJamType.Int32), DataMember(Name = "startLocationX")]
		public int StartLocationX
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "faction", Type = FlexJamType.Struct), DataMember(Name = "faction")]
		public MobileWorldQuestReward[] Faction
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "objective", Type = FlexJamType.Struct), DataMember(Name = "objective")]
		public MobileWorldQuestObjective[] Objective
		{
			get;
			set;
		}

		[FlexJamMember(Name = "questTitle", Type = FlexJamType.String), DataMember(Name = "questTitle")]
		public string QuestTitle
		{
			get;
			set;
		}
	}
}
