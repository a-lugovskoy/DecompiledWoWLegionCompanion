using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4851, Name = "MobileClientShipmentsUpdate", Version = 28333852u), DataContract]
	public class MobileClientShipmentsUpdate
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "shipment", Type = FlexJamType.Struct), DataMember(Name = "shipment")]
		public JamCharacterShipment[] Shipment
		{
			get;
			set;
		}
	}
}
