using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayDeliverable", Version = 28333852u), DataContract]
	public class JamBattlePayDeliverable
	{
		[FlexJamMember(Name = "alreadyOwns", Type = FlexJamType.Bool), DataMember(Name = "alreadyOwns")]
		public bool AlreadyOwns
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "choices", Type = FlexJamType.Struct), DataMember(Name = "choices")]
		public JamBattlePayDeliverableChoice[] Choices
		{
			get;
			set;
		}

		[FlexJamMember(Name = "deliverableID", Type = FlexJamType.UInt32), DataMember(Name = "deliverableID")]
		public uint DeliverableID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "battlePetCreatureID", Type = FlexJamType.UInt32), DataMember(Name = "battlePetCreatureID")]
		public uint BattlePetCreatureID
		{
			get;
			set;
		}

		[FlexJamMember(Optional = true, Name = "petResult", Type = FlexJamType.Enum), DataMember(Name = "petResult")]
		public BATTLEPETRESULT[] PetResult
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

		[FlexJamMember(Optional = true, Name = "displayInfo", Type = FlexJamType.Struct), DataMember(Name = "displayInfo")]
		public JamBattlepayDisplayInfo[] DisplayInfo
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemID", Type = FlexJamType.UInt32), DataMember(Name = "itemID")]
		public uint ItemID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "type", Type = FlexJamType.UInt8), DataMember(Name = "type")]
		public byte Type
		{
			get;
			set;
		}

		[FlexJamMember(Name = "mountSpellID", Type = FlexJamType.UInt32), DataMember(Name = "mountSpellID")]
		public uint MountSpellID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "quantity", Type = FlexJamType.UInt32), DataMember(Name = "quantity")]
		public uint Quantity
		{
			get;
			set;
		}
	}
}
