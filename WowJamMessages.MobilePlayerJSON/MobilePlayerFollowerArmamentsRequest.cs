using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4803, Name = "MobilePlayerFollowerArmamentsRequest", Version = 28333852u), DataContract]
	public class MobilePlayerFollowerArmamentsRequest
	{
		[FlexJamMember(Name = "garrFollowerTypeID", Type = FlexJamType.Int32), DataMember(Name = "garrFollowerTypeID")]
		public int GarrFollowerTypeID
		{
			get;
			set;
		}
	}
}
