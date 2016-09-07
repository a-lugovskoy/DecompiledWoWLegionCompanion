using System;
using System.Runtime.Serialization;

namespace JamLib
{
	[FlexJamStruct(Name = "WowTime"), DataContract]
	public struct WowTime
	{
		[FlexJamMember(Name = "minute", Type = FlexJamType.Int32), DataMember(Name = "minute")]
		public int Minute
		{
			get;
			set;
		}

		[FlexJamMember(Name = "hour", Type = FlexJamType.Int32), DataMember(Name = "hour")]
		public int Hour
		{
			get;
			set;
		}

		[FlexJamMember(Name = "weekday", Type = FlexJamType.Int32), DataMember(Name = "weekday")]
		public int WeekDay
		{
			get;
			set;
		}

		[FlexJamMember(Name = "monthDay", Type = FlexJamType.Int32), DataMember(Name = "monthDay")]
		public int MonthDay
		{
			get;
			set;
		}

		[FlexJamMember(Name = "month", Type = FlexJamType.Int32), DataMember(Name = "month")]
		public int Month
		{
			get;
			set;
		}

		[FlexJamMember(Name = "year", Type = FlexJamType.Int32), DataMember(Name = "year")]
		public int Year
		{
			get;
			set;
		}

		[FlexJamMember(Name = "flags", Type = FlexJamType.Int32), DataMember(Name = "flags")]
		public int Flags
		{
			get;
			set;
		}

		[FlexJamMember(Name = "holidayOffset", Type = FlexJamType.Int32), DataMember(Name = "holidayOffset")]
		public int HolidayOffset
		{
			get;
			set;
		}
	}
}
