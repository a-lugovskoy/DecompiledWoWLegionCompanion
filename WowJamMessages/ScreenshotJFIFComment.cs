using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "ScreenshotJFIFComment", Version = 28333852u), DataContract]
	public class ScreenshotJFIFComment
	{
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string Guid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "level", Type = FlexJamType.Int32), DataMember(Name = "level")]
		public int Level
		{
			get;
			set;
		}

		[FlexJamMember(Name = "raceID", Type = FlexJamType.UInt32), DataMember(Name = "raceID")]
		public uint RaceID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "worldport", Type = FlexJamType.String), DataMember(Name = "worldport")]
		public string Worldport
		{
			get;
			set;
		}

		[FlexJamMember(Name = "isInGame", Type = FlexJamType.Bool), DataMember(Name = "isInGame")]
		public bool IsInGame
		{
			get;
			set;
		}

		[FlexJamMember(Name = "realmName", Type = FlexJamType.String), DataMember(Name = "realmName")]
		public string RealmName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "zoneName", Type = FlexJamType.String), DataMember(Name = "zoneName")]
		public string ZoneName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "facing", Type = FlexJamType.Float), DataMember(Name = "facing")]
		public float Facing
		{
			get;
			set;
		}

		[FlexJamMember(Name = "mapID", Type = FlexJamType.UInt32), DataMember(Name = "mapID")]
		public uint MapID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "position", Type = FlexJamType.Struct), DataMember(Name = "position")]
		public Vector3 Position
		{
			get;
			set;
		}

		[FlexJamMember(Name = "name", Type = FlexJamType.String), DataMember(Name = "name")]
		public string Name
		{
			get;
			set;
		}

		[FlexJamMember(Name = "classID", Type = FlexJamType.UInt32), DataMember(Name = "classID")]
		public uint ClassID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "sex", Type = FlexJamType.UInt32), DataMember(Name = "sex")]
		public uint Sex
		{
			get;
			set;
		}

		[FlexJamMember(Name = "mapName", Type = FlexJamType.String), DataMember(Name = "mapName")]
		public string MapName
		{
			get;
			set;
		}
	}
}
