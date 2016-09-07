using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamCurrencySimple", Version = 28333852u), DataContract]
	public class JamCurrencySimple
	{
		[FlexJamMember(Name = "type", Type = FlexJamType.Int32), DataMember(Name = "type")]
		public int Type
		{
			get;
			set;
		}

		[FlexJamMember(Name = "quantity", Type = FlexJamType.Int32), DataMember(Name = "quantity")]
		public int Quantity
		{
			get;
			set;
		}
	}
}
