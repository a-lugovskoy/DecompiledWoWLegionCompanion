using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileClientShipmentType", Version = 28333852u), DataContract]
	public class MobileClientShipmentType
	{
		[FlexJamMember(Name = "currencyTypeID", Type = FlexJamType.Int32), DataMember(Name = "currencyTypeID")]
		public int CurrencyTypeID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "charShipmentID", Type = FlexJamType.Int32), DataMember(Name = "charShipmentID")]
		public int CharShipmentID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "currencyCost", Type = FlexJamType.Int32), DataMember(Name = "currencyCost")]
		public int CurrencyCost
		{
			get;
			set;
		}
	}
}
