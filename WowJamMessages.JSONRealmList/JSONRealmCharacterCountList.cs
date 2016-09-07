using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamMessage(Id = 15030, Name = "JSONRealmCharacterCountList", Version = 28333852u), DataContract]
	public class JSONRealmCharacterCountList
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "counts", Type = FlexJamType.Struct), DataMember(Name = "counts")]
		public JamJSONRealmCharacterCount[] Counts
		{
			get;
			set;
		}
	}
}
