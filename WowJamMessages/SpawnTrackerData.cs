using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "SpawnTrackerData", Version = 28333852u), DataContract]
	public class SpawnTrackerData
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "questID", Type = FlexJamType.Int32), DataMember(Name = "questID")]
		public int[] QuestID
		{
			get;
			set;
		}
	}
}
