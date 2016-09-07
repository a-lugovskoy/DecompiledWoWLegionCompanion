using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamMessage(Id = 15031, Name = "JSONRealmListUpdates", Version = 28333852u), DataContract]
	public class JSONRealmListUpdates
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "updates", Type = FlexJamType.Struct), DataMember(Name = "updates")]
		public JamJSONRealmListUpdatePart[] Updates
		{
			get;
			set;
		}
	}
}
