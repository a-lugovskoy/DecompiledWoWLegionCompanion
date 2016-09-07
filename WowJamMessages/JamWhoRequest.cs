using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamWhoRequest", Version = 28333852u), DataContract]
	public class JamWhoRequest
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "words", Type = FlexJamType.Struct), DataMember(Name = "words")]
		public JamWhoWord[] Words
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "serverInfo", Type = FlexJamType.Struct), DataMember(Name = "serverInfo")]
		public JamWhoRequestServerInfo[] ServerInfo
		{
			get;
			set;
		}

		[FlexJamMember(Name = "minLevel", Type = FlexJamType.Int32), DataMember(Name = "minLevel")]
		public int MinLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "classFilter", Type = FlexJamType.Int32), DataMember(Name = "classFilter")]
		public int ClassFilter
		{
			get;
			set;
		}

		[FlexJamMember(Name = "showEnemies", Type = FlexJamType.Bool), DataMember(Name = "showEnemies")]
		public bool ShowEnemies
		{
			get;
			set;
		}

		[FlexJamMember(Name = "guildVirtualRealmName", Type = FlexJamType.String), DataMember(Name = "guildVirtualRealmName")]
		public string GuildVirtualRealmName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "showArenaPlayers", Type = FlexJamType.Bool), DataMember(Name = "showArenaPlayers")]
		public bool ShowArenaPlayers
		{
			get;
			set;
		}

		[FlexJamMember(Name = "maxLevel", Type = FlexJamType.Int32), DataMember(Name = "maxLevel")]
		public int MaxLevel
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

		[FlexJamMember(Name = "guild", Type = FlexJamType.String), DataMember(Name = "guild")]
		public string Guild
		{
			get;
			set;
		}

		[FlexJamMember(Name = "raceFilter", Type = FlexJamType.Int32), DataMember(Name = "raceFilter")]
		public int RaceFilter
		{
			get;
			set;
		}

		[FlexJamMember(Name = "virtualRealmName", Type = FlexJamType.String), DataMember(Name = "virtualRealmName")]
		public string VirtualRealmName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "exactName", Type = FlexJamType.Bool), DataMember(Name = "exactName")]
		public bool ExactName
		{
			get;
			set;
		}
	}
}
