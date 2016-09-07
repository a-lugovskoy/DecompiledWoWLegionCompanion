using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4852, Name = "MobileClientShipmentTypes", Version = 28333852u), DataContract]
	public class MobileClientShipmentTypes
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "shipment", Type = FlexJamType.Struct), DataMember(Name = "shipment")]
		public MobileClientShipmentType[] Shipment
		{
			get;
			set;
		}
	}
}
