using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONGameVersion", Version = 28333852u), DataContract]
	public class JamJSONGameVersion
	{
		[FlexJamMember(Name = "versionMajor", Type = FlexJamType.UInt32), DataMember(Name = "versionMajor")]
		public uint VersionMajor
		{
			get;
			set;
		}

		[FlexJamMember(Name = "versionBuild", Type = FlexJamType.UInt32), DataMember(Name = "versionBuild")]
		public uint VersionBuild
		{
			get;
			set;
		}

		[FlexJamMember(Name = "versionMinor", Type = FlexJamType.UInt32), DataMember(Name = "versionMinor")]
		public uint VersionMinor
		{
			get;
			set;
		}

		[FlexJamMember(Name = "versionRevision", Type = FlexJamType.UInt32), DataMember(Name = "versionRevision")]
		public uint VersionRevision
		{
			get;
			set;
		}
	}
}
