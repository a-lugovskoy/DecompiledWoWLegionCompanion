using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[FlexJamMessage(Id = 4742, Name = "MobileServerSetCharacter", Version = 28333852u), DataContract]
	public class MobileServerSetCharacter
	{
		[FlexJamMember(Name = "characterGUID", Type = FlexJamType.WowGuid), DataMember(Name = "characterGUID")]
		public string CharacterGUID
		{
			get;
			set;
		}
	}
}
