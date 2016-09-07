using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamSpawnRegionPlayerActivity", Version = 28333852u), DataContract]
	public class JamSpawnRegionPlayerActivity
	{
		[FlexJamMember(Name = "idleTime", Type = FlexJamType.UInt32), DataMember(Name = "idleTime")]
		public uint IdleTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "player", Type = FlexJamType.WowGuid), DataMember(Name = "player")]
		public string Player
		{
			get;
			set;
		}

		[FlexJamMember(Name = "activeTime", Type = FlexJamType.UInt32), DataMember(Name = "activeTime")]
		public uint ActiveTime
		{
			get;
			set;
		}
	}
}
