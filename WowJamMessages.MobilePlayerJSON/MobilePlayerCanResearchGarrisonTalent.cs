using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4799, Name = "MobilePlayerCanResearchGarrisonTalent", Version = 28333852u), DataContract]
	public class MobilePlayerCanResearchGarrisonTalent
	{
		[FlexJamMember(Name = "garrTalentID", Type = FlexJamType.Int32), DataMember(Name = "garrTalentID")]
		public int GarrTalentID
		{
			get;
			set;
		}
	}
}
