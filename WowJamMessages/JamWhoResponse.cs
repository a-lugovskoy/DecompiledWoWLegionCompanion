using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamWhoResponse", Version = 28333852u), DataContract]
	public class JamWhoResponse
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "entries", Type = FlexJamType.Struct), DataMember(Name = "entries")]
		public JamWhoEntry[] Entries
		{
			get;
			set;
		}
	}
}
