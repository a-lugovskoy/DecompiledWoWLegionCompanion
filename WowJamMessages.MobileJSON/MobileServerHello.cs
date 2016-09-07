using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[DataContract]
	public class MobileServerHello
	{
		[DataMember(Name = "token")]
		public ulong Token
		{
			get;
			set;
		}

		[DataMember(Name = "reply")]
		public bool Reply
		{
			get;
			set;
		}

		[DataMember(Name = "versionMajor")]
		public uint VersionMajor
		{
			get;
			set;
		}

		[DataMember(Name = "versionMinor")]
		public uint VersionMinor
		{
			get;
			set;
		}

		[DataMember(Name = "versionRevision")]
		public uint VersionRevision
		{
			get;
			set;
		}

		[DataMember(Name = "versionBuild")]
		public uint VersionBuild
		{
			get;
			set;
		}

		[DataMember(Name = "protocolVersion")]
		public uint ProtocolVersion
		{
			get;
			set;
		}

		[DataMember(Name = "appName")]
		public string AppName
		{
			get;
			set;
		}

		[DataMember(Name = "appPath")]
		public string AppPath
		{
			get;
			set;
		}

		[DataMember(Name = "machineName")]
		public string MachineName
		{
			get;
			set;
		}
	}
}
