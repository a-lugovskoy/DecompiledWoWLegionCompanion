using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamPartyMemberState", Version = 28333852u), DataContract]
	public class JamPartyMemberState
	{
		[FlexJamMember(Name = "level", Type = FlexJamType.UInt16), DataMember(Name = "level")]
		public ushort Level
		{
			get;
			set;
		}

		[FlexJamMember(Name = "overrideDisplayPower", Type = FlexJamType.UInt16), DataMember(Name = "overrideDisplayPower")]
		public ushort OverrideDisplayPower
		{
			get;
			set;
		}

		[FlexJamMember(Name = "maxHealth", Type = FlexJamType.Int32), DataMember(Name = "maxHealth")]
		public int MaxHealth
		{
			get;
			set;
		}

		[FlexJamMember(Name = "maxPower", Type = FlexJamType.UInt16), DataMember(Name = "maxPower")]
		public ushort MaxPower
		{
			get;
			set;
		}

		[FlexJamMember(Name = "displayPower", Type = FlexJamType.UInt8), DataMember(Name = "displayPower")]
		public byte DisplayPower
		{
			get;
			set;
		}

		[FlexJamMember(Name = "wmoDoodadPlacementID", Type = FlexJamType.UInt32), DataMember(Name = "wmoDoodadPlacementID")]
		public uint WmoDoodadPlacementID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "auras", Type = FlexJamType.Struct), DataMember(Name = "auras")]
		public JamPartyMemberAuraState[] Auras
		{
			get;
			set;
		}

		[FlexJamMember(Name = "spec", Type = FlexJamType.UInt16), DataMember(Name = "spec")]
		public ushort Spec
		{
			get;
			set;
		}

		[FlexJamMember(Name = "vehicleSeatRecID", Type = FlexJamType.Int32), DataMember(Name = "vehicleSeatRecID")]
		public int VehicleSeatRecID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "phase", Type = FlexJamType.Struct), DataMember(Name = "phase")]
		public PhaseShiftData Phase
		{
			get;
			set;
		}

		[FlexJamMember(Name = "loc", Type = FlexJamType.Struct), DataMember(Name = "loc")]
		public JamShortVec3 Loc
		{
			get;
			set;
		}

		[FlexJamMember(Name = "areaID", Type = FlexJamType.UInt16), DataMember(Name = "areaID")]
		public ushort AreaID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "wmoGroupID", Type = FlexJamType.UInt16), DataMember(Name = "wmoGroupID")]
		public ushort WmoGroupID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "health", Type = FlexJamType.Int32), DataMember(Name = "health")]
		public int Health
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "partyType", Type = FlexJamType.UInt8), DataMember(Name = "partyType")]
		public byte[] PartyType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "flags", Type = FlexJamType.UInt16), DataMember(Name = "flags")]
		public ushort Flags
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "pet", Type = FlexJamType.Struct), DataMember(Name = "pet")]
		public JamPartyMemberPetState[] Pet
		{
			get;
			set;
		}

		[FlexJamMember(Name = "power", Type = FlexJamType.UInt16), DataMember(Name = "power")]
		public ushort Power
		{
			get;
			set;
		}

		public JamPartyMemberState()
		{
			this.PartyType = new byte[2];
		}
	}
}
