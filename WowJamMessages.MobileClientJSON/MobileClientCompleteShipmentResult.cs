using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4853, Name = "MobileClientCompleteShipmentResult", Version = 28333852u), DataContract]
	public class MobileClientCompleteShipmentResult
	{
		[FlexJamMember(Name = "shipmentID", Type = FlexJamType.UInt64), DataMember(Name = "shipmentID")]
		public ulong ShipmentID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "result", Type = FlexJamType.Int32), DataMember(Name = "result")]
		public int Result
		{
			get;
			set;
		}
	}
}
