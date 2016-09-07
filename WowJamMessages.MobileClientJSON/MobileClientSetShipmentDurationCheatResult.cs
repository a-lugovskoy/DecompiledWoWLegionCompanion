using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4857, Name = "MobileClientSetShipmentDurationCheatResult", Version = 28333852u), DataContract]
	public class MobileClientSetShipmentDurationCheatResult
	{
		[FlexJamMember(Name = "success", Type = FlexJamType.Bool), DataMember(Name = "success")]
		public bool Success
		{
			get;
			set;
		}
	}
}
