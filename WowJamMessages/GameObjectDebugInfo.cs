using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "GameObjectDebugInfo", Version = 28333852u), DataContract]
	public class GameObjectDebugInfo
	{
		[FlexJamMember(Name = "health", Type = FlexJamType.Float), DataMember(Name = "health")]
		public float Health
		{
			get;
			set;
		}

		[FlexJamMember(Name = "state", Type = FlexJamType.Int32), DataMember(Name = "state")]
		public int State
		{
			get;
			set;
		}

		[FlexJamMember(Name = "flags", Type = FlexJamType.UInt32), DataMember(Name = "flags")]
		public uint Flags
		{
			get;
			set;
		}

		[FlexJamMember(Name = "gameObjectType", Type = FlexJamType.Int32), DataMember(Name = "gameObjectType")]
		public int GameObjectType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "debugName", Type = FlexJamType.String), DataMember(Name = "debugName")]
		public string DebugName
		{
			get;
			set;
		}
	}
}
