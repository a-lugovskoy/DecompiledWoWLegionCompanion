using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4849, Name = "MobileClientEmissaryFactionUpdate", Version = 28333852u), DataContract]
	public class MobileClientEmissaryFactionUpdate
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "faction", Type = FlexJamType.Struct), DataMember(Name = "faction")]
		public MobileEmissaryFaction[] Faction
		{
			get;
			set;
		}
	}
}
