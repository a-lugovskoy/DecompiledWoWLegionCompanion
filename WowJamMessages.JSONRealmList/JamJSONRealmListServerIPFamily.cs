using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmListServerIPFamily", Version = 28333852u), DataContract]
	public class JamJSONRealmListServerIPFamily
	{
		[FlexJamMember(Name = "family", Type = FlexJamType.Int8), DataMember(Name = "family")]
		public sbyte Family
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "addresses", Type = FlexJamType.Struct), DataMember(Name = "addresses")]
		public JamJSONRealmListServerIPAddress[] Addresses
		{
			get;
			set;
		}
	}
}
