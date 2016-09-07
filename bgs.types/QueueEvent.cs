using System;

namespace bgs.types
{
	public class QueueEvent
	{
		public enum Type
		{
			UNKNOWN = 0,
			QUEUE_ENTER = 1,
			QUEUE_LEAVE = 2,
			QUEUE_DELAY = 3,
			QUEUE_UPDATE = 4,
			QUEUE_DELAY_ERROR = 5,
			QUEUE_AMM_ERROR = 6,
			QUEUE_WAIT_END = 7,
			QUEUE_CANCEL = 8,
			QUEUE_GAME_STARTED = 9,
			ABORT_CLIENT_DROPPED = 10
		}

		public QueueEvent.Type EventType
		{
			get;
			set;
		}

		public int MinSeconds
		{
			get;
			set;
		}

		public int MaxSeconds
		{
			get;
			set;
		}

		public int BnetError
		{
			get;
			set;
		}

		public GameServerInfo GameServer
		{
			get;
			set;
		}

		public QueueEvent(QueueEvent.Type t, int minSeconds, int maxSeconds, int bnetError, GameServerInfo gsInfo)
		{
			this.EventType = t;
			this.MinSeconds = minSeconds;
			this.MaxSeconds = maxSeconds;
			this.BnetError = bnetError;
			this.GameServer = gsInfo;
		}
	}
}
