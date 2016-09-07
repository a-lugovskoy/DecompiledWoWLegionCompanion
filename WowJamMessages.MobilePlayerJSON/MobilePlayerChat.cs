using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4789, Name = "MobilePlayerChat", Version = 28333852u), DataContract]
	public class MobilePlayerChat
	{
		[FlexJamMember(Name = "slashCmd", Type = FlexJamType.UInt8), DataMember(Name = "slashCmd")]
		public byte SlashCmd
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

		[FlexJamMember(Name = "targetName", Type = FlexJamType.String), DataMember(Name = "targetName")]
		public string TargetName
		{
			get;
			set;
		}

		public MobilePlayerChat()
		{
			this.TargetName = string.Empty;
			this.ChatText = string.Empty;
		}
	}
}
