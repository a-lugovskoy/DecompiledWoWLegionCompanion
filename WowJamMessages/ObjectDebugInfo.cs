using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "ObjectDebugInfo", Version = 28333852u), DataContract]
	public class ObjectDebugInfo
	{
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string Guid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "rawFacing", Type = FlexJamType.Float), DataMember(Name = "rawFacing")]
		public float RawFacing
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "gameObjectDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "gameObjectDebugInfo")]
		public GameObjectDebugInfo[] GameObjectDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "attributeDescriptions", Type = FlexJamType.Struct), DataMember(Name = "attributeDescriptions")]
		public DebugAttributeDescription[] AttributeDescriptions
		{
			get;
			set;
		}

		[FlexJamMember(Name = "updateTime", Type = FlexJamType.Int32), DataMember(Name = "updateTime")]
		public int UpdateTime
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "attributes", Type = FlexJamType.Struct), DataMember(Name = "attributes")]
		public DebugAttribute[] Attributes
		{
			get;
			set;
		}

		[FlexJamMember(Name = "mapID", Type = FlexJamType.Int32), DataMember(Name = "mapID")]
		public int MapID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "typeID", Type = FlexJamType.Int32), DataMember(Name = "typeID")]
		public int TypeID
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

		[FlexJamMember(Name = "rawPosition", Type = FlexJamType.Struct), DataMember(Name = "rawPosition")]
		public Vector3 RawPosition
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "scriptTableValueDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "scriptTableValueDebugInfo")]
		public ScriptTableValueDebugInfo[] ScriptTableValueDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "phaseInfo", Type = FlexJamType.Struct), DataMember(Name = "phaseInfo")]
		public ObjectPhaseDebugInfo[] PhaseInfo
		{
			get;
			set;
		}

		[FlexJamMember(Name = "ID", Type = FlexJamType.Int32), DataMember(Name = "ID")]
		public int ID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "initialized", Type = FlexJamType.Bool), DataMember(Name = "initialized")]
		public bool Initialized
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "playerDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "playerDebugInfo")]
		public PlayerDebugInfo[] PlayerDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "unitDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "unitDebugInfo")]
		public UnitDebugInfo[] UnitDebugInfo
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

		[FlexJamMember(Name = "name", Type = FlexJamType.String), DataMember(Name = "name")]
		public string Name
		{
			get;
			set;
		}

		public ObjectDebugInfo()
		{
			this.Initialized = false;
			this.MapID = 0;
		}
	}
}
