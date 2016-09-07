using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamQualifiedGUID", Version = 28333852u), DataContract]
	public class JamQualifiedGUID
	{
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid), DataMember(Name = "guid")]
		public string Guid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "virtualRealmAddress", Type = FlexJamType.UInt32), DataMember(Name = "virtualRealmAddress")]
		public uint VirtualRealmAddress
		{
			get;
			set;
		}

		public JamQualifiedGUID()
		{
			this.VirtualRealmAddress = 0u;
			this.Guid = "0000000000000000";
		}
	}
}
