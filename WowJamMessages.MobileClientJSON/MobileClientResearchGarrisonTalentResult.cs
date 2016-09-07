using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4859, Name = "MobileClientResearchGarrisonTalentResult", Version = 28333852u), DataContract]
	public class MobileClientResearchGarrisonTalentResult
	{
		[FlexJamMember(Name = "garrTalentID", Type = FlexJamType.Int32), DataMember(Name = "garrTalentID")]
		public int GarrTalentID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "result", Type = FlexJamType.Int32), DataMember(Name = "result")]
		public int Result
		{
			get;
			set;
		}
	}
}
