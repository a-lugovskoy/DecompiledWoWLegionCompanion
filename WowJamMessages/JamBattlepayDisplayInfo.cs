using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlepayDisplayInfo", Version = 28333852u), DataContract]
	public class JamBattlepayDisplayInfo
	{
		[FlexJamMember(Name = "name1", Type = FlexJamType.String), DataMember(Name = "name1")]
		public string Name1
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "overrideTextColor", Type = FlexJamType.UInt32), DataMember(Name = "overrideTextColor")]
		public uint[] OverrideTextColor
		{
			get;
			set;
		}

		[FlexJamMember(Name = "name2", Type = FlexJamType.String), DataMember(Name = "name2")]
		public string Name2
		{
			get;
			set;
		}

		[FlexJamMember(Name = "name3", Type = FlexJamType.String), DataMember(Name = "name3")]
		public string Name3
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "overrideBackground", Type = FlexJamType.UInt32), DataMember(Name = "overrideBackground")]
		public uint[] OverrideBackground
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "overrideTexture", Type = FlexJamType.UInt32), DataMember(Name = "overrideTexture")]
		public uint[] OverrideTexture
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "flags", Type = FlexJamType.UInt32), DataMember(Name = "flags")]
		public uint[] Flags
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "creatureDisplayInfoID", Type = FlexJamType.UInt32), DataMember(Name = "creatureDisplayInfoID")]
		public uint[] CreatureDisplayInfoID
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "fileDataID", Type = FlexJamType.UInt32), DataMember(Name = "fileDataID")]
		public uint[] FileDataID
		{
			get;
			set;
		}
	}
}
