using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlenetRequestHeader", Version = 28333852u), DataContract]
	public class JamBattlenetRequestHeader
	{
		[FlexJamMember(Name = "methodType", Type = FlexJamType.UInt64), DataMember(Name = "methodType")]
		public ulong MethodType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "token", Type = FlexJamType.UInt32), DataMember(Name = "token")]
		public uint Token
		{
			get;
			set;
		}

		[FlexJamMember(Name = "objectID", Type = FlexJamType.UInt64), DataMember(Name = "objectID")]
		public ulong ObjectID
		{
			get;
			set;
		}

		public JamBattlenetRequestHeader()
		{
			this.ObjectID = 0uL;
		}
	}
}
