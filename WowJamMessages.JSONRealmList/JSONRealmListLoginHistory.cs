using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamMessage(Id = 15032, Name = "JSONRealmListLoginHistory", Version = 28333852u), DataContract]
	public class JSONRealmListLoginHistory
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "history", Type = FlexJamType.Struct), DataMember(Name = "history")]
		public JamJSONRealmListLoginHistoryEntry[] History
		{
			get;
			set;
		}
	}
}
