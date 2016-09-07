using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGameTime", Version = 28333852u), DataContract]
	public class JamGameTime
	{
		[FlexJamMember(Name = "minutesRemaining", Type = FlexJamType.UInt32), DataMember(Name = "minutesRemaining")]
		public uint MinutesRemaining
		{
			get;
			set;
		}

		[FlexJamMember(Name = "isInIGR", Type = FlexJamType.Bool), DataMember(Name = "isInIGR")]
		public bool IsInIGR
		{
			get;
			set;
		}

		[FlexJamMember(Name = "isCAISEnabled", Type = FlexJamType.Bool), DataMember(Name = "isCAISEnabled")]
		public bool IsCAISEnabled
		{
			get;
			set;
		}

		[FlexJamMember(Name = "billingType", Type = FlexJamType.Int32), DataMember(Name = "billingType")]
		public int BillingType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "isPaidForByIGR", Type = FlexJamType.Bool), DataMember(Name = "isPaidForByIGR")]
		public bool IsPaidForByIGR
		{
			get;
			set;
		}

		public JamGameTime()
		{
			this.BillingType = 0;
			this.MinutesRemaining = 0u;
			this.IsInIGR = false;
			this.IsPaidForByIGR = false;
			this.IsCAISEnabled = false;
		}
	}
}
