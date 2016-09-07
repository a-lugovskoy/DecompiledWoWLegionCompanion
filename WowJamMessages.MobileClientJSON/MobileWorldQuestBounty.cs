using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileWorldQuestBounty", Version = 28333852u), DataContract]
	public class MobileWorldQuestBounty
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

		[FlexJamMember(Name = "numNeeded", Type = FlexJamType.Int32), DataMember(Name = "numNeeded")]
		public int NumNeeded
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

		[FlexJamMember(Name = "questID", Type = FlexJamType.Int32), DataMember(Name = "questID")]
		public int QuestID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "iconFileDataID", Type = FlexJamType.Int32), DataMember(Name = "iconFileDataID")]
		public int IconFileDataID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "numCompleted", Type = FlexJamType.Int32), DataMember(Name = "numCompleted")]
		public int NumCompleted
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

		[FlexJamMember(ArrayDimensions = 1, Name = "currency", Type = FlexJamType.Struct), DataMember(Name = "currency")]
		public MobileWorldQuestReward[] Currency
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
	}
}
