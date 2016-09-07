using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4862, Name = "MobileClientFollowerArmamentsResult", Version = 28333852u), DataContract]
	public class MobileClientFollowerArmamentsResult
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "armament", Type = FlexJamType.Struct), DataMember(Name = "armament")]
		public MobileFollowerArmament[] Armament
		{
			get;
			set;
		}
	}
}
