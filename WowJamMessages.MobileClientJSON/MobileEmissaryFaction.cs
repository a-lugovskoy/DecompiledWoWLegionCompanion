using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileEmissaryFaction", Version = 28333852u), DataContract]
	public class MobileEmissaryFaction
	{
		[FlexJamMember(Name = "factionID", Type = FlexJamType.UInt16), DataMember(Name = "factionID")]
		public ushort FactionID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "factionAmount", Type = FlexJamType.Int32), DataMember(Name = "factionAmount")]
		public int FactionAmount
		{
			get;
			set;
		}
	}
}
