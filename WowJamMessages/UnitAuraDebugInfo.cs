using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "UnitAuraDebugInfo", Version = 28333852u), DataContract]
	public class UnitAuraDebugInfo
	{
		[FlexJamMember(Name = "spellID", Type = FlexJamType.Int32), DataMember(Name = "spellID")]
		public int SpellID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "fromItemSet", Type = FlexJamType.Bool), DataMember(Name = "fromItemSet")]
		public bool FromItemSet
		{
			get;
			set;
		}

		[FlexJamMember(Name = "serverOnly", Type = FlexJamType.Bool), DataMember(Name = "serverOnly")]
		public bool ServerOnly
		{
			get;
			set;
		}

		[FlexJamMember(Name = "fromEnchantment", Type = FlexJamType.Bool), DataMember(Name = "fromEnchantment")]
		public bool FromEnchantment
		{
			get;
			set;
		}

		[FlexJamMember(Name = "enchantmentSlot", Type = FlexJamType.Int32), DataMember(Name = "enchantmentSlot")]
		public int EnchantmentSlot
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "effectDebugInfo", Type = FlexJamType.Struct), DataMember(Name = "effectDebugInfo")]
		public UnitAuraEffectDebugInfo[] EffectDebugInfo
		{
			get;
			set;
		}

		[FlexJamMember(Name = "paused", Type = FlexJamType.Bool), DataMember(Name = "paused")]
		public bool Paused
		{
			get;
			set;
		}

		[FlexJamMember(Name = "itemName", Type = FlexJamType.String), DataMember(Name = "itemName")]
		public string ItemName
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

		[FlexJamMember(Name = "itemID", Type = FlexJamType.Int32), DataMember(Name = "itemID")]
		public int ItemID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "fromItem", Type = FlexJamType.Bool), DataMember(Name = "fromItem")]
		public bool FromItem
		{
			get;
			set;
		}
	}
}
