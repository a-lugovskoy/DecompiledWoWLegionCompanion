using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4844, Name = "MobileClientChat", Version = 28333852u), DataContract]
	public class MobileClientChat
	{
		[FlexJamMember(Name = "senderName", Type = FlexJamType.String), DataMember(Name = "senderName")]
		public string SenderName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "senderGUID", Type = FlexJamType.WowGuid), DataMember(Name = "senderGUID")]
		public string SenderGUID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "chatText", Type = FlexJamType.String), DataMember(Name = "chatText")]
		public string ChatText
		{
			get;
			set;
		}

		[FlexJamMember(Name = "prefix", Type = FlexJamType.String), DataMember(Name = "prefix")]
		public string Prefix
		{
			get;
			set;
		}

		[FlexJamMember(Name = "channel", Type = FlexJamType.String), DataMember(Name = "channel")]
		public string Channel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "slashCmd", Type = FlexJamType.UInt8), DataMember(Name = "slashCmd")]
		public byte SlashCmd
		{
			get;
			set;
		}

		[FlexJamMember(Name = "chatFlags", Type = FlexJamType.UInt16), DataMember(Name = "chatFlags")]
		public ushort ChatFlags
		{
			get;
			set;
		}

		public MobileClientChat()
		{
			this.SenderGUID = "0000000000000000";
			this.SenderName = string.Empty;
			this.Prefix = string.Empty;
			this.Channel = string.Empty;
			this.ChatText = string.Empty;
			this.ChatFlags = 0;
		}
	}
}
