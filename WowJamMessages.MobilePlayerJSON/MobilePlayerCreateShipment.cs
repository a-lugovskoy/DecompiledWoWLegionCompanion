using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4791, Name = "MobilePlayerCreateShipment", Version = 28333852u), DataContract]
	public class MobilePlayerCreateShipment
	{
		[FlexJamMember(Name = "charShipmentID", Type = FlexJamType.Int32), DataMember(Name = "charShipmentID")]
		public int CharShipmentID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "numShipments", Type = FlexJamType.Int32), DataMember(Name = "numShipments")]
		public int NumShipments
		{
			get;
			set;
		}
	}
}
