using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "UnitDebugInfo", Version = 28333852u), DataContract]
	public class UnitDebugInfo
	{
		[FlexJamMember(Name = "level", Type = FlexJamType.Int32), DataMember(Name = "level")]
		public int Level
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "aiTriggerActionDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "aiTriggerActionDebugInfo")]
		public AITriggerActionDebugInfo[] AiTriggerActionDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "spellDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "spellDebugInfo")]
		public CreatureSpellDebugInfo[] SpellDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "effectiveStatValues", Type = FlexJamType.Int32), DataMember(Name = "effectiveStatValues")]
		public int[] EffectiveStatValues
		{
			get;
			set;
		}

		[FlexJamMember(Name = "spawnRegionName", Type = FlexJamType.String), DataMember(Name = "spawnRegionName")]
		public string SpawnRegionName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "spawnGroupName", Type = FlexJamType.String), DataMember(Name = "spawnGroupName")]
		public string SpawnGroupName
		{
			get;
			set;
		}

		[FlexJamMember(Name = "spawnRegionID", Type = FlexJamType.Int32), DataMember(Name = "spawnRegionID")]
		public int SpawnRegionID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "classID", Type = FlexJamType.Int32), DataMember(Name = "classID")]
		public int ClassID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "aiTriggerActionSetDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "aiTriggerActionSetDebugInfo")]
		public AITriggerActionSetDebugInfo[] AiTriggerActionSetDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(Name = "playerClassID", Type = FlexJamType.Int32), DataMember(Name = "playerClassID")]
		public int PlayerClassID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "percentSupportAction", Type = FlexJamType.Int32), DataMember(Name = "percentSupportAction")]
		public int PercentSupportAction
		{
			get;
			set;
		}

		[FlexJamMember(Name = "spawnGroupID", Type = FlexJamType.Int32), DataMember(Name = "spawnGroupID")]
		public int SpawnGroupID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "spawnEventDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "spawnEventDebugInfo")]
		public SpawnEventDebugInfo[] SpawnEventDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "auraDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "auraDebugInfo")]
		public UnitAuraDebugInfo[] AuraDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(Name = "creatureSpellDataID", Type = FlexJamType.Int32), DataMember(Name = "creatureSpellDataID")]
		public int CreatureSpellDataID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "zoneFlags", Type = FlexJamType.UInt32), DataMember(Name = "zoneFlags")]
		public uint[] ZoneFlags
		{
			get;
			set;
		}

		[FlexJamMember(Name = "percentRangedAttack", Type = FlexJamType.Int32), DataMember(Name = "percentRangedAttack")]
		public int PercentRangedAttack
		{
			get;
			set;
		}

		public UnitDebugInfo()
		{
			this.ZoneFlags = new uint[3];
			this.EffectiveStatValues = new int[5];
			this.CreatureSpellDataID = 0;
			this.PercentSupportAction = 0;
			this.PercentRangedAttack = 0;
			this.SpawnGroupID = 0;
			this.SpawnGroupName = string.Empty;
			this.SpawnRegionID = 0;
			this.SpawnRegionName = string.Empty;
		}
	}
}
