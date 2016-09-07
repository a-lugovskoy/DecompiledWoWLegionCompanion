using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmListTicketClientInformation", Version = 28333852u), DataContract]
	public class JamJSONRealmListTicketClientInformation
	{
		[FlexJamMember(Name = "platform", Type = FlexJamType.UInt32), DataMember(Name = "platform")]
		public uint Platform
		{
			get;
			set;
		}

		[FlexJamMember(Name = "currentTime", Type = FlexJamType.Int32), DataMember(Name = "currentTime")]
		public int CurrentTime
		{
			get;
			set;
		}

		[FlexJamMember(Name = "buildVariant", Type = FlexJamType.String), DataMember(Name = "buildVariant")]
		public string BuildVariant
		{
			get;
			set;
		}

		[FlexJamMember(Name = "timeZone", Type = FlexJamType.String), DataMember(Name = "timeZone")]
		public string TimeZone
		{
			get;
			set;
		}

		[FlexJamMember(Name = "versionDataBuild", Type = FlexJamType.UInt32), DataMember(Name = "versionDataBuild")]
		public uint VersionDataBuild
		{
			get;
			set;
		}

		[FlexJamMember(Name = "audioLocale", Type = FlexJamType.UInt32), DataMember(Name = "audioLocale")]
		public uint AudioLocale
		{
			get;
			set;
		}

		[FlexJamMember(Name = "version", Type = FlexJamType.Struct), DataMember(Name = "version")]
		public JamJSONGameVersion Version
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "secret", Type = FlexJamType.UInt8), DataMember(Name = "secret")]
		public byte[] Secret
		{
			get;
			set;
		}

		[FlexJamMember(Name = "type", Type = FlexJamType.UInt32), DataMember(Name = "type")]
		public uint Type
		{
			get;
			set;
		}

		[FlexJamMember(Name = "textLocale", Type = FlexJamType.UInt32), DataMember(Name = "textLocale")]
		public uint TextLocale
		{
			get;
			set;
		}

		public JamJSONRealmListTicketClientInformation()
		{
			this.Secret = new byte[32];
		}
	}
}
