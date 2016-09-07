using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4804, Name = "MobilePlayerUseFollowerArmament", Version = 28333852u), DataContract]
	public class MobilePlayerUseFollowerArmament
	{
		[FlexJamMember(Name = "garrFollowerTypeID", Type = FlexJamType.Int32), DataMember(Name = "garrFollowerTypeID")]
		public int GarrFollowerTypeID
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

		[FlexJamMember(Name = "itemID", Type = FlexJamType.Int32), DataMember(Name = "itemID")]
		public int ItemID
		{
			get;
			set;
		}
	}
}
