using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmEntry", Version = 28333852u), DataContract]
	public class JamJSONRealmEntry
	{
		[FlexJamMember(Name = "wowRealmAddress", Type = FlexJamType.UInt32), DataMember(Name = "wowRealmAddress")]
		public uint WowRealmAddress
		{
			get;
			set;
		}

		[FlexJamMember(Name = "cfgTimezonesID", Type = FlexJamType.Int32), DataMember(Name = "cfgTimezonesID")]
		public int CfgTimezonesID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "populationState", Type = FlexJamType.Int32), DataMember(Name = "populationState")]
		public int PopulationState
		{
			get;
			set;
		}

		[FlexJamMember(Name = "cfgCategoriesID", Type = FlexJamType.Int32), DataMember(Name = "cfgCategoriesID")]
		public int CfgCategoriesID
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

		[FlexJamMember(Name = "cfgRealmsID", Type = FlexJamType.Int32), DataMember(Name = "cfgRealmsID")]
		public int CfgRealmsID
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

		[FlexJamMember(Name = "name", Type = FlexJamType.String), DataMember(Name = "name")]
		public string Name
		{
			get;
			set;
		}

		[FlexJamMember(Name = "cfgConfigsID", Type = FlexJamType.Int32), DataMember(Name = "cfgConfigsID")]
		public int CfgConfigsID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "cfgLanguagesID", Type = FlexJamType.Int32), DataMember(Name = "cfgLanguagesID")]
		public int CfgLanguagesID
		{
			get;
			set;
		}
	}
}
