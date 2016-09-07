using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamDumpObjectInfo", Version = 28333852u), DataContract]
	public class JamDumpObjectInfo
	{
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string Guid
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

		[FlexJamMember(Name = "granted", Type = FlexJamType.Bool), DataMember(Name = "granted")]
		public bool Granted
		{
			get;
			set;
		}

		[FlexJamMember(Name = "visibleRange", Type = FlexJamType.Float), DataMember(Name = "visibleRange")]
		public float VisibleRange
		{
			get;
			set;
		}

		[FlexJamMember(Name = "displayID", Type = FlexJamType.UInt32), DataMember(Name = "displayID")]
		public uint DisplayID
		{
			get;
			set;
		}

		public JamDumpObjectInfo()
		{
			this.Granted = true;
		}
	}
}
