using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamMessage(Id = 15036, Name = "JSONRealmCharacterList", Version = 28333852u), DataContract]
	public class JSONRealmCharacterList
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "characterList", Type = FlexJamType.Struct), DataMember(Name = "characterList")]
		public JamJSONCharacterEntry[] CharacterList
		{
			get;
			set;
		}
	}
}
