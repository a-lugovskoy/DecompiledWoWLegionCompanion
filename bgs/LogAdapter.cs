using bgs.Wrapper.Impl;
using System;
using System.Collections.Generic;

namespace bgs
{
	public class LogAdapter
	{
		private static LoggerInterface s_impl = new DefaultLogger();

		public static void SetLogger<T>(T outputter) where T : LoggerInterface, new()
		{
			LogAdapter.s_impl = outputter;
		}

		public static void Log(LogLevel logLevel, string str)
		{
			LogAdapter.s_impl.Log(logLevel, str);
		}

		public static List<string> GetLogEvents()
		{
			return LogAdapter.s_impl.GetLogEvents();
		}

		public static void ClearLogEvents()
		{
			LogAdapter.s_impl.ClearLogEvents();
		}
	}
}
