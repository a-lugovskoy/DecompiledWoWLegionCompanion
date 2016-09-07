using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4865, Name = "MobileClientFollowerActivationDataResult", Version = 28333852u), DataContract]
	public class MobileClientFollowerActivationDataResult
	{
		[FlexJamMember(Name = "goldCost", Type = FlexJamType.Int32), DataMember(Name = "goldCost")]
		public int GoldCost
		{
			get;
			set;
		}

		[FlexJamMember(Name = "activationsRemaining", Type = FlexJamType.Int32), DataMember(Name = "activationsRemaining")]
		public int ActivationsRemaining
		{
			get;
			set;
		}
	}
}
