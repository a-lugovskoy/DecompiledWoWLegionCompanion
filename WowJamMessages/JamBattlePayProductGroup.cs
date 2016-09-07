using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayProductGroup", Version = 28333852u), DataContract]
	public class JamBattlePayProductGroup
	{
		[FlexJamMember(Name = "iconFileDataID", Type = FlexJamType.Int32), DataMember(Name = "iconFileDataID")]
		public int IconFileDataID
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

		[FlexJamMember(Name = "displayType", Type = FlexJamType.UInt8), DataMember(Name = "displayType")]
		public byte DisplayType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "ordering", Type = FlexJamType.Int32), DataMember(Name = "ordering")]
		public int Ordering
		{
			get;
			set;
		}

		[FlexJamMember(Name = "groupID", Type = FlexJamType.UInt32), DataMember(Name = "groupID")]
		public uint GroupID
		{
			get;
			set;
		}
	}
}
