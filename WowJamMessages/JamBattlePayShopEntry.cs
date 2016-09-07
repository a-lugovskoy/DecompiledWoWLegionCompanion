using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayShopEntry", Version = 28333852u), DataContract]
	public class JamBattlePayShopEntry
	{
		[FlexJamMember(Name = "entryID", Type = FlexJamType.UInt32), DataMember(Name = "entryID")]
		public uint EntryID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "flags", Type = FlexJamType.UInt32), DataMember(Name = "flags")]
		public uint Flags
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "displayInfo", Type = FlexJamType.Struct), DataMember(Name = "displayInfo")]
		public JamBattlepayDisplayInfo[] DisplayInfo
		{
			get;
			set;
		}

		[FlexJamMember(Name = "ordering", Type = FlexJamType.Int32), DataMember(Name = "ordering")]
		public int Ordering
		{
			get;
			set;
		}

		[FlexJamMember(Name = "bannerType", Type = FlexJamType.UInt8), DataMember(Name = "bannerType")]
		public byte BannerType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "productID", Type = FlexJamType.UInt32), DataMember(Name = "productID")]
		public uint ProductID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "groupID", Type = FlexJamType.UInt32), DataMember(Name = "groupID")]
		public uint GroupID
		{
			get;
			set;
		}
	}
}
