using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4850, Name = "MobileClientCreateShipmentResult", Version = 28333852u), DataContract]
	public class MobileClientCreateShipmentResult
	{
		[FlexJamMember(Name = "charShipmentID", Type = FlexJamType.Int32), DataMember(Name = "charShipmentID")]
		public int CharShipmentID
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
