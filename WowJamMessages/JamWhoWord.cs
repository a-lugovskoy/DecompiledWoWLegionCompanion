using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamWhoWord", Version = 28333852u), DataContract]
	public class JamWhoWord
	{
		[FlexJamMember(Name = "word", Type = FlexJamType.String), DataMember(Name = "word")]
		public string Word
		{
			get;
			set;
		}
	}
}
