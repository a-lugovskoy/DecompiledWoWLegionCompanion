using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4848, Name = "MobileClientAccountCharacters", Version = 28333852u), DataContract]
	public class MobileClientAccountCharacters
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "characters", Type = FlexJamType.Struct), DataMember(Name = "characters")]
		public MobilePlayerCharacter[] Characters
		{
			get;
			set;
		}
	}
}
