using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4806, Name = "MobilePlayerFollowerActivationDataRequest", Version = 28333852u), DataContract]
	public class MobilePlayerFollowerActivationDataRequest
	{
		[FlexJamMember(Name = "garrTypeID", Type = FlexJamType.Int32), DataMember(Name = "garrTypeID")]
		public int GarrTypeID
		{
			get;
			set;
		}
	}
}
