using System;
using System.Runtime.InteropServices;

namespace bgs.types
{
	public struct FriendsUpdate
	{
		public enum Action
		{
			UNKNOWN = 0,
			FRIEND_ADDED = 1,
			FRIEND_REMOVED = 2,
			FRIEND_INVITE = 3,
			FRIEND_INVITE_REMOVED = 4,
			FRIEND_SENT_INVITE = 5,
			FRIEND_SENT_INVITE_REMOVED = 6,
			FRIEND_ROLE_CHANGE = 7,
			FRIEND_ATTR_CHANGE = 8,
			FRIEND_GAME_ADDED = 9,
			FRIEND_GAME_REMOVED = 10
		}

		public int action;

		public BnetEntityId entity1;

		public BnetEntityId entity2;

		public int int1;

		public string string1;

		public string string2;

		public string string3;

		public ulong long1;

		public ulong long2;

		public ulong long3;

		[MarshalAs(3)]
		public bool bool1;
	}
}
