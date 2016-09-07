using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4807, Name = "MobilePlayerChangeFollowerActive", Version = 28333852u), DataContract]
	public class MobilePlayerChangeFollowerActive
	{
		[FlexJamMember(Name = "setInactive", Type = FlexJamType.Bool), DataMember(Name = "setInactive")]
		public bool SetInactive
		{
			get;
			set;
		}

		[FlexJamMember(Name = "garrFollowerID", Type = FlexJamType.Int32), DataMember(Name = "garrFollowerID")]
		public int GarrFollowerID
		{
			get;
			set;
		}
	}
}
