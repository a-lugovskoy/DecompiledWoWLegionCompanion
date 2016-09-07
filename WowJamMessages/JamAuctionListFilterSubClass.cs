using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamAuctionListFilterSubClass", Version = 28333852u), DataContract]
	public class JamAuctionListFilterSubClass
	{
		[FlexJamMember(Name = "itemSubclass", Type = FlexJamType.Int32), DataMember(Name = "itemSubclass")]
		public int ItemSubclass
		{
			get;
			set;
		}

		[FlexJamMember(Name = "invTypeMask", Type = FlexJamType.UInt32), DataMember(Name = "invTypeMask")]
		public uint InvTypeMask
		{
			get;
			set;
		}
	}
}
