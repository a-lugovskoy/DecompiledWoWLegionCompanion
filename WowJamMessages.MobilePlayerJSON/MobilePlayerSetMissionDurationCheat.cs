using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4797, Name = "MobilePlayerSetMissionDurationCheat", Version = 28333852u), DataContract]
	public class MobilePlayerSetMissionDurationCheat
	{
		[FlexJamMember(Name = "seconds", Type = FlexJamType.Int32), DataMember(Name = "seconds")]
		public int Seconds
		{
			get;
			set;
		}
	}
}
