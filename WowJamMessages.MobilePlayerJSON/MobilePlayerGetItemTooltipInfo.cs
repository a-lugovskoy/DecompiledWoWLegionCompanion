using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4810, Name = "MobilePlayerGetItemTooltipInfo", Version = 28333852u), DataContract]
	public class MobilePlayerGetItemTooltipInfo
	{
		[FlexJamMember(Name = "itemContext", Type = FlexJamType.Int32), DataMember(Name = "itemContext")]
		public int ItemContext
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
