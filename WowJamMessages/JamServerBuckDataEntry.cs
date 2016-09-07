using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamServerBuckDataEntry", Version = 28333852u), DataContract]
	public class JamServerBuckDataEntry
	{
		[FlexJamMember(Name = "accum", Type = FlexJamType.UInt64), DataMember(Name = "accum")]
		public ulong Accum
		{
			get;
			set;
		}

		[FlexJamMember(Name = "maximum", Type = FlexJamType.UInt64), DataMember(Name = "maximum")]
		public ulong Maximum
		{
			get;
			set;
		}

		[FlexJamMember(Name = "sqaccum", Type = FlexJamType.UInt64), DataMember(Name = "sqaccum")]
		public ulong Sqaccum
		{
			get;
			set;
		}

		[FlexJamMember(Name = "arg", Type = FlexJamType.UInt64), DataMember(Name = "arg")]
		public ulong Arg
		{
			get;
			set;
		}

		[FlexJamMember(Name = "count", Type = FlexJamType.UInt64), DataMember(Name = "count")]
		public ulong Count
		{
			get;
			set;
		}

		[FlexJamMember(Name = "argname", Type = FlexJamType.String), DataMember(Name = "argname")]
		public string Argname
		{
			get;
			set;
		}

		[FlexJamMember(Name = "minimum", Type = FlexJamType.UInt64), DataMember(Name = "minimum")]
		public ulong Minimum
		{
			get;
			set;
		}

		public JamServerBuckDataEntry()
		{
			this.Arg = 0uL;
			this.Argname = string.Empty;
			this.Count = 0uL;
			this.Accum = 0uL;
			this.Sqaccum = 0uL;
			this.Maximum = 0uL;
			this.Minimum = 2000000000uL;
		}
	}
}
