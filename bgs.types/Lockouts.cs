using System;
using System.Runtime.InteropServices;

namespace bgs.types
{
	public struct Lockouts
	{
		[MarshalAs(3)]
		public bool loaded;

		[MarshalAs(3)]
		public bool loading;

		[MarshalAs(3)]
		public bool readingPCI;

		[MarshalAs(3)]
		public bool readingGTRI;

		[MarshalAs(3)]
		public bool readingCAISI;

		[MarshalAs(3)]
		public bool readingGSI;

		[MarshalAs(3)]
		public bool parentalControls;

		[MarshalAs(3)]
		public bool parentalTimedAccount;

		public int parentalMinutesRemaining;

		public IntPtr day1;

		public IntPtr day2;

		public IntPtr day3;

		public IntPtr day4;

		public IntPtr day5;

		public IntPtr day6;

		public IntPtr day7;

		[MarshalAs(3)]
		public bool timedAccount;

		public int minutesRemaining;

		public ulong sessionStartTime;

		[MarshalAs(3)]
		public bool CAISactive;

		public int CAISplayed;

		public int CAISrested;
	}
}
