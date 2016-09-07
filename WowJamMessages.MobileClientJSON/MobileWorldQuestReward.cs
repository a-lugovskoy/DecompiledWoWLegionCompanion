using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileWorldQuestReward", Version = 28333852u), DataContract]
	public class MobileWorldQuestReward
	{
		[FlexJamMember(Name = "itemContext", Type = FlexJamType.Int32), DataMember(Name = "itemContext")]
		public int ItemContext
		{
			get;
			set;
		}

		[FlexJamMember(Name = "recordID", Type = FlexJamType.Int32), DataMember(Name = "recordID")]
		public int RecordID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "fileDataID", Type = FlexJamType.Int32), DataMember(Name = "fileDataID")]
		public int FileDataID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "quantity", Type = FlexJamType.Int32), DataMember(Name = "quantity")]
		public int Quantity
		{
			get;
			set;
		}

		public MobileWorldQuestReward()
		{
			this.FileDataID = 0;
			this.ItemContext = 0;
		}
	}
}
