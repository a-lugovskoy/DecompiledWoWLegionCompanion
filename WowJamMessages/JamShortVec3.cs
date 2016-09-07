using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamShortVec3", Version = 28333852u), DataContract]
	public class JamShortVec3
	{
		[FlexJamMember(Name = "z", Type = FlexJamType.Int16), DataMember(Name = "z")]
		public short Z
		{
			get;
			set;
		}

		[FlexJamMember(Name = "x", Type = FlexJamType.Int16), DataMember(Name = "x")]
		public short X
		{
			get;
			set;
		}

		[FlexJamMember(Name = "y", Type = FlexJamType.Int16), DataMember(Name = "y")]
		public short Y
		{
			get;
			set;
		}
	}
}
