using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamServerBuckDataList", Version = 28333852u), DataContract]
	public class JamServerBuckDataList
	{
		[FlexJamMember(Name = "mpID", Type = FlexJamType.UInt32), DataMember(Name = "mpID")]
		public uint MpID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "entries", Type = FlexJamType.Struct), DataMember(Name = "entries")]
		public JamServerBuckDataEntry[] Entries
		{
			get;
			set;
		}
	}
}
