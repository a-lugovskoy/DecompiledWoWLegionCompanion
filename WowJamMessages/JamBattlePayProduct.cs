using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayProduct", Version = 28333852u), DataContract]
	public class JamBattlePayProduct
	{
		[FlexJamMember(Name = "currentPriceFixedPoint", Type = FlexJamType.UInt64), DataMember(Name = "currentPriceFixedPoint")]
		public ulong CurrentPriceFixedPoint
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

		[FlexJamMember(Name = "normalPriceFixedPoint", Type = FlexJamType.UInt64), DataMember(Name = "normalPriceFixedPoint")]
		public ulong NormalPriceFixedPoint
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

		[FlexJamMember(Name = "type", Type = FlexJamType.UInt8), DataMember(Name = "type")]
		public byte Type
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "deliverables", Type = FlexJamType.UInt32), DataMember(Name = "deliverables")]
		public uint[] Deliverables
		{
			get;
			set;
		}
	}
}
