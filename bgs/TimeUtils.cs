using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace bgs
{
	public class TimeUtils
	{
		public enum ElapsedTimeType
		{
			SECONDS = 0,
			MINUTES = 1,
			HOURS = 2,
			YESTERDAY = 3,
			DAYS = 4,
			WEEKS = 5,
			MONTH_AGO = 6
		}

		public class ElapsedStringSet
		{
			public string m_seconds;

			public string m_minutes;

			public string m_hours;

			public string m_yesterday;

			public string m_days;

			public string m_weeks;

			public string m_monthAgo;
		}

		public const int SEC_PER_MINUTE = 60;

		public const int SEC_PER_HOUR = 3600;

		public const int SEC_PER_DAY = 86400;

		public const int SEC_PER_WEEK = 604800;

		public const int MS_PER_SEC = 1000;

		public const int MS_PER_MINUTE = 60000;

		public const int MS_PER_HOUR = 3600000;

		public const string DEFAULT_TIME_UNITS_STR = "sec";

		public static readonly DateTime EPOCH_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 1);

		public static readonly TimeUtils.ElapsedStringSet SPLASHSCREEN_DATETIME_STRINGSET = new TimeUtils.ElapsedStringSet
		{
			m_seconds = "GLOBAL_DATETIME_SPLASHSCREEN_SECONDS",
			m_minutes = "GLOBAL_DATETIME_SPLASHSCREEN_MINUTES",
			m_hours = "GLOBAL_DATETIME_SPLASHSCREEN_HOURS",
			m_yesterday = "GLOBAL_DATETIME_SPLASHSCREEN_DAY",
			m_days = "GLOBAL_DATETIME_SPLASHSCREEN_DAYS",
			m_weeks = "GLOBAL_DATETIME_SPLASHSCREEN_WEEKS",
			m_monthAgo = "GLOBAL_DATETIME_SPLASHSCREEN_MONTH"
		};

		public static long BinaryStamp()
		{
			return DateTime.get_UtcNow().ToBinary();
		}

		public static DateTime ConvertEpochMicrosecToDateTime(ulong microsec)
		{
			return TimeUtils.EPOCH_TIME.AddMilliseconds(microsec / 1000.0);
		}

		public static TimeSpan GetElapsedTimeSinceEpoch(DateTime? endDateTime = null)
		{
			DateTime dateTime = (!endDateTime.get_HasValue()) ? DateTime.get_UtcNow() : endDateTime.get_Value();
			return dateTime - TimeUtils.EPOCH_TIME;
		}

		public static void GetElapsedTime(int seconds, out TimeUtils.ElapsedTimeType timeType, out int time)
		{
			time = 0;
			if (seconds < 60)
			{
				timeType = TimeUtils.ElapsedTimeType.SECONDS;
				time = seconds;
				return;
			}
			if (seconds < 3600)
			{
				timeType = TimeUtils.ElapsedTimeType.MINUTES;
				time = seconds / 60;
				return;
			}
			int num = seconds / 86400;
			if (num == 0)
			{
				timeType = TimeUtils.ElapsedTimeType.HOURS;
				time = seconds / 3600;
				return;
			}
			if (num == 1)
			{
				timeType = TimeUtils.ElapsedTimeType.YESTERDAY;
				return;
			}
			int num2 = seconds / 604800;
			if (num2 == 0)
			{
				timeType = TimeUtils.ElapsedTimeType.DAYS;
				time = num;
				return;
			}
			if (num2 < 4)
			{
				timeType = TimeUtils.ElapsedTimeType.WEEKS;
				time = num2;
				return;
			}
			timeType = TimeUtils.ElapsedTimeType.MONTH_AGO;
		}

		public static string GetDevElapsedTimeString(TimeSpan span)
		{
			return TimeUtils.GetDevElapsedTimeString((long)span.get_TotalMilliseconds());
		}

		public static string GetDevElapsedTimeString(long ms)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			if (ms >= 3600000L)
			{
				TimeUtils.AppendDevTimeUnitsString("{0}h", 3600000, stringBuilder, ref ms, ref num);
			}
			if (ms >= 60000L)
			{
				TimeUtils.AppendDevTimeUnitsString("{0}m", 60000, stringBuilder, ref ms, ref num);
			}
			if (ms >= 1000L)
			{
				TimeUtils.AppendDevTimeUnitsString("{0}s", 1000, stringBuilder, ref ms, ref num);
			}
			if (num <= 1)
			{
				TimeUtils.AppendDevTimeUnitsString("{0}ms", 1, stringBuilder, ref ms, ref num);
			}
			return stringBuilder.ToString();
		}

		public static bool TryParseDevSecFromElapsedTimeString(string timeStr, out float sec)
		{
			sec = 0f;
			MatchCollection matchCollection = Regex.Matches(timeStr, "(?<number>(?:[0-9]+,)*[0-9]+)\\s*(?<units>[a-zA-Z]+)");
			if (matchCollection.get_Count() == 0)
			{
				return false;
			}
			Match match = matchCollection.get_Item(0);
			if (!match.get_Groups().get_Item(0).get_Success())
			{
				return false;
			}
			Group group = match.get_Groups().get_Item("number");
			Group group2 = match.get_Groups().get_Item("units");
			if (!group.get_Success() || !group2.get_Success())
			{
				return false;
			}
			string value = group.get_Value();
			string text = group2.get_Value();
			if (!float.TryParse(value, ref sec))
			{
				return false;
			}
			text = TimeUtils.ParseTimeUnitsStr(text);
			if (text == "min")
			{
				sec *= 60f;
			}
			else if (text == "hour")
			{
				sec *= 3600f;
			}
			return true;
		}

		public static float ForceDevSecFromElapsedTimeString(string timeStr)
		{
			float result;
			TimeUtils.TryParseDevSecFromElapsedTimeString(timeStr, out result);
			return result;
		}

		private static void AppendDevTimeUnitsString(string formatString, int msPerUnit, StringBuilder builder, ref long ms, ref int unitCount)
		{
			long num = ms / (long)msPerUnit;
			if (num > 0L)
			{
				if (unitCount > 0)
				{
					builder.Append(' ');
				}
				builder.AppendFormat(formatString, num);
				unitCount++;
			}
			ms -= num * (long)msPerUnit;
		}

		private static string ParseTimeUnitsStr(string unitsStr)
		{
			if (unitsStr == null)
			{
				return "sec";
			}
			unitsStr = unitsStr.ToLowerInvariant();
			string text = unitsStr;
			if (text != null)
			{
				if (TimeUtils.<>f__switch$mapE == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(13);
					dictionary.Add("s", 0);
					dictionary.Add("sec", 0);
					dictionary.Add("secs", 0);
					dictionary.Add("second", 0);
					dictionary.Add("seconds", 0);
					dictionary.Add("m", 1);
					dictionary.Add("min", 1);
					dictionary.Add("mins", 1);
					dictionary.Add("minute", 1);
					dictionary.Add("minutes", 1);
					dictionary.Add("h", 2);
					dictionary.Add("hour", 2);
					dictionary.Add("hours", 2);
					TimeUtils.<>f__switch$mapE = dictionary;
				}
				int num;
				if (TimeUtils.<>f__switch$mapE.TryGetValue(text, ref num))
				{
					switch (num)
					{
					case 0:
						return "sec";
					case 1:
						return "min";
					case 2:
						return "hour";
					}
				}
			}
			return "sec";
		}
	}
}
