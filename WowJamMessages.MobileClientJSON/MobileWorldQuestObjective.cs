using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileWorldQuestObjective", Version = 28333852u), DataContract]
	public class MobileWorldQuestObjective
	{
		[FlexJamMember(Name = "text", Type = FlexJamType.String), DataMember(Name = "text")]
		public string Text
		{
			get;
			set;
		}
	}
}
