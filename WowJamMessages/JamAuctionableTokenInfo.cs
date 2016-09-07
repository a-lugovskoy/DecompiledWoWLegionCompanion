using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamAuctionableTokenInfo", Version = 28333852u), DataContract]
	public class JamAuctionableTokenInfo
	{
		[FlexJamMember(Name = "price", Type = FlexJamType.UInt64), DataMember(Name = "price")]
		public ulong Price
		{
			get;
			set;
		}

		[FlexJamMember(Name = "status", Type = FlexJamType.Int32), DataMember(Name = "status")]
		public int Status
		{
			get;
			set;
		}

		[FlexJamMember(Name = "expectedSecondsUntilSold", Type = FlexJamType.UInt32), DataMember(Name = "expectedSecondsUntilSold")]
		public uint ExpectedSecondsUntilSold
		{
			get;
			set;
		}

		[FlexJamMember(Name = "id", Type = FlexJamType.UInt64), DataMember(Name = "id")]
		public ulong Id
		{
			get;
			set;
		}

		[FlexJamMember(Name = "lastUpdate", Type = FlexJamType.Int32), DataMember(Name = "lastUpdate")]
		public int LastUpdate
		{
			get;
			set;
		}
	}
}
