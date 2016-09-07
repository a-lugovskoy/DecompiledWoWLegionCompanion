using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.friends
{
	public class ViewFriendsResponse : IProtoBuf
	{
		private List<Friend> _Friends = new List<Friend>();

		public List<Friend> Friends
		{
			get
			{
				return this._Friends;
			}
			set
			{
				this._Friends = value;
			}
		}

		public List<Friend> FriendsList
		{
			get
			{
				return this._Friends;
			}
		}

		public int FriendsCount
		{
			get
			{
				return this._Friends.get_Count();
			}
		}

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			ViewFriendsResponse.Deserialize(stream, this);
		}

		public static ViewFriendsResponse Deserialize(Stream stream, ViewFriendsResponse instance)
		{
			return ViewFriendsResponse.Deserialize(stream, instance, -1L);
		}

		public static ViewFriendsResponse DeserializeLengthDelimited(Stream stream)
		{
			ViewFriendsResponse viewFriendsResponse = new ViewFriendsResponse();
			ViewFriendsResponse.DeserializeLengthDelimited(stream, viewFriendsResponse);
			return viewFriendsResponse;
		}

		public static ViewFriendsResponse DeserializeLengthDelimited(Stream stream, ViewFriendsResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ViewFriendsResponse.Deserialize(stream, instance, num);
		}

		public static ViewFriendsResponse Deserialize(Stream stream, ViewFriendsResponse instance, long limit)
		{
			if (instance.Friends == null)
			{
				instance.Friends = new List<Friend>();
			}
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num == -1)
				{
					if (limit >= 0L)
					{
						throw new EndOfStreamException();
					}
					return instance;
				}
				else
				{
					int num2 = num;
					if (num2 != 10)
					{
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						uint field = key.Field;
						if (field == 0u)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					else
					{
						instance.Friends.Add(Friend.DeserializeLengthDelimited(stream));
					}
				}
			}
			if (stream.get_Position() == limit)
			{
				return instance;
			}
			throw new ProtocolBufferException("Read past max limit");
		}

		public void Serialize(Stream stream)
		{
			ViewFriendsResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ViewFriendsResponse instance)
		{
			if (instance.Friends.get_Count() > 0)
			{
				using (List<Friend>.Enumerator enumerator = instance.Friends.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Friend current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						Friend.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Friends.get_Count() > 0)
			{
				using (List<Friend>.Enumerator enumerator = this.Friends.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Friend current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddFriends(Friend val)
		{
			this._Friends.Add(val);
		}

		public void ClearFriends()
		{
			this._Friends.Clear();
		}

		public void SetFriends(List<Friend> val)
		{
			this.Friends = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<Friend>.Enumerator enumerator = this.Friends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Friend current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ViewFriendsResponse viewFriendsResponse = obj as ViewFriendsResponse;
			if (viewFriendsResponse == null)
			{
				return false;
			}
			if (this.Friends.get_Count() != viewFriendsResponse.Friends.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Friends.get_Count(); i++)
			{
				if (!this.Friends.get_Item(i).Equals(viewFriendsResponse.Friends.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static ViewFriendsResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ViewFriendsResponse>(bs, 0, -1);
		}
	}
}
