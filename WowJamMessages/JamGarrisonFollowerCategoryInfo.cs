using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonFollowerCategoryInfo", Version = 28333852u), DataContract]
	public class JamGarrisonFollowerCategoryInfo
	{
		[FlexJamMember(Name = "classSpec", Type = FlexJamType.Int32), DataMember(Name = "classSpec")]
		public int ClassSpec
		{
			get;
			set;
		}

		[FlexJamMember(Name = "classSpecPlayerCondID", Type = FlexJamType.Int32), DataMember(Name = "classSpecPlayerCondID")]
		public int ClassSpecPlayerCondID
		{
			get;
			set;
		}
	}
}
