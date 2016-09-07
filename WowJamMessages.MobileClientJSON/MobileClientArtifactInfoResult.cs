using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4868, Name = "MobileClientArtifactInfoResult", Version = 28333852u), DataContract]
	public class MobileClientArtifactInfoResult
	{
		[FlexJamMember(Name = "knowledgeLevel", Type = FlexJamType.Int32), DataMember(Name = "knowledgeLevel")]
		public int KnowledgeLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "xpMultiplier", Type = FlexJamType.Float), DataMember(Name = "xpMultiplier")]
		public float XpMultiplier
		{
			get;
			set;
		}
	}
}
