using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamCharacterShipment", Version = 28333852u), DataContract]
	public class JamCharacterShipment
	{
		[FlexJamMember(Name = "shipmentDuration", Type = FlexJamType.Int32), DataMember(Name = "shipmentDuration")]
		public int ShipmentDuration
		{
			get;
			set;
		}

		[FlexJamMember(Name = "assignedFollowerDBID", Type = FlexJamType.UInt64), DataMember(Name = "assignedFollowerDBID")]
		public ulong AssignedFollowerDBID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "shipmentID", Type = FlexJamType.UInt64), DataMember(Name = "shipmentID")]
		public ulong ShipmentID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "shipmentRecID", Type = FlexJamType.Int32), DataMember(Name = "shipmentRecID")]
		public int ShipmentRecID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "creationTime", Type = FlexJamType.Int32), DataMember(Name = "creationTime")]
		public int CreationTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "buildingType", Type = FlexJamType.Int32), DataMember(Name = "buildingType")]
		public int BuildingType
		{
			get;
			set;
		}
	}
}
