using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamAuctionListFilterClass", Version = 28333852u), DataContract]
	public class JamAuctionListFilterClass
	{
		[FlexJamMember(Name = "itemClass", Type = FlexJamType.Int32), DataMember(Name = "itemClass")]
		public int ItemClass
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "subClasses", Type = FlexJamType.Struct), DataMember(Name = "subClasses")]
		public JamAuctionListFilterSubClass[] SubClasses
		{
			get;
			set;
		}
	}
}
