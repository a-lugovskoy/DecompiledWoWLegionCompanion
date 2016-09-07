using System;
using System.Runtime.Serialization;

namespace JamLib
{
	[FlexJamStruct(Name = "vector2"), DataContract]
	public struct Vector2
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
	}
}
