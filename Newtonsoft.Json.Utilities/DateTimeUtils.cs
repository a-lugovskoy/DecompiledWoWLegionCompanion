using System;
using System.Globalization;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
	internal static class DateTimeUtils
	{
		public static string GetLocalOffset(this DateTime d)
		{
			TimeSpan utcOffset = TimeZoneInfo.get_Local().GetUtcOffset(d);
			return utcOffset.get_Hours().ToString("+00;-00", CultureInfo.get_InvariantCulture()) + ":" + utcOffset.get_Minutes().ToString("00;00", CultureInfo.get_InvariantCulture());
		}

		public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
		{
			switch (kind)
			{
			case 0:
				return 2;
			case 1:
				return 1;
			case 2:
				return 0;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
			}
		}
	}
}
