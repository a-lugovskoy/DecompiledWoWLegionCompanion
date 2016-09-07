using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4867, Name = "MobileClientGetItemTooltipInfoResult", Version = 28333852u), DataContract]
	public class MobileClientGetItemTooltipInfoResult
	{
		[FlexJamMember(Name = "itemContext", Type = FlexJamType.Int32), DataMember(Name = "itemContext")]
		public int ItemContext
		{
			get;
			set;
		}

		[FlexJamMember(Name = "stats", Type = FlexJamType.Struct), DataMember(Name = "stats")]
		public MobileItemStats Stats
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemID", Type = FlexJamType.Int32), DataMember(Name = "itemID")]
		public int ItemID
		{
			get;
			set;
		}
	}
}
