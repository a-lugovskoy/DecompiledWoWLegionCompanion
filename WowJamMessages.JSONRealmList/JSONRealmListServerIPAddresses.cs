using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamMessage(Id = 15033, Name = "JSONRealmListServerIPAddresses", Version = 28333852u), DataContract]
	public class JSONRealmListServerIPAddresses
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "families", Type = FlexJamType.Struct), DataMember(Name = "families")]
		public JamJSONRealmListServerIPFamily[] Families
		{
			get;
			set;
		}
	}
}
