using System;
using System.Collections.Generic;

namespace bgs.Wrapper.Impl
{
	internal class DefaultLogger : LoggerInterface
	{
		private static List<string> s_logEvents = new List<string>();

		public void Log(LogLevel logLevel, string str)
		{
			Console.WriteLine(str);
			DefaultLogger.s_logEvents.Add(str);
		}

		public List<string> GetLogEvents()
		{
			return DefaultLogger.s_logEvents;
		}

		public void ClearLogEvents()
		{
			DefaultLogger.s_logEvents.Clear();
		}
	}
}
