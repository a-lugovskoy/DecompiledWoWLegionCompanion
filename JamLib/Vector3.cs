using System;
using System.Runtime.Serialization;

namespace JamLib
{
	[FlexJamStruct(Name = "vector3"), DataContract]
	public struct Vector3
	{
		[FlexJamMember(Name = "x", Type = FlexJamType.Float), DataMember(Name = "x")]
		public float X
		{
			get;
			set;
		}

		[FlexJamMember(Name = "y", Type = FlexJamType.Float), DataMember(Name = "y")]
		public float Y
		{
			get;
			set;
		}

		[FlexJamMember(Name = "z", Type = FlexJamType.Float), DataMember(Name = "z")]
		public float Z
		{
			get;
			set;
		}
	}
}
