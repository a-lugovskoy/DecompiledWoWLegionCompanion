using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamShipmentData", Version = 28333852u), DataContract]
	public class JamShipmentData
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "shipment", Type = FlexJamType.Struct), DataMember(Name = "shipment")]
		public JamCharacterShipment[] Shipment
		{
			get;
			set;
		}

		[FlexJamMember(Name = "resetPending", Type = FlexJamType.Bool), DataMember(Name = "resetPending")]
		public bool ResetPending
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "pendingShipment", Type = FlexJamType.Int32), DataMember(Name = "pendingShipment")]
		public int[] PendingShipment
		{
			get;
			set;
		}

		public JamShipmentData()
		{
			this.ResetPending = false;
		}
	}
}
