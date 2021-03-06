using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel
{
	public class AddNotification : IProtoBuf
	{
		public bool HasSelf;

		private Member _Self;

		private List<Member> _Member = new List<Member>();

		public Member Self
		{
			get
			{
				return this._Self;
			}
			set
			{
				this._Self = value;
				this.HasSelf = (value != null);
			}
		}

		public List<Member> Member
		{
			get
			{
				return this._Member;
			}
			set
			{
				this._Member = value;
			}
		}

		public List<Member> MemberList
		{
			get
			{
				return this._Member;
			}
		}

		public int MemberCount
		{
			get
			{
				return this._Member.get_Count();
			}
		}

		public ChannelState ChannelState
		{
			get;
			set;
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
			AddNotification.Deserialize(stream, this);
		}

		public static AddNotification Deserialize(Stream stream, AddNotification instance)
		{
			return AddNotification.Deserialize(stream, instance, -1L);
		}

		public static AddNotification DeserializeLengthDelimited(Stream stream)
		{
			AddNotification addNotification = new AddNotification();
			AddNotification.DeserializeLengthDelimited(stream, addNotification);
			return addNotification;
		}

		public static AddNotification DeserializeLengthDelimited(Stream stream, AddNotification instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return AddNotification.Deserialize(stream, instance, num);
		}

		public static AddNotification Deserialize(Stream stream, AddNotification instance, long limit)
		{
			if (instance.Member == null)
			{
				instance.Member = new List<Member>();
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
						if (num2 != 18)
						{
							if (num2 != 26)
							{
								Key key = ProtocolParser.ReadKey((byte)num, stream);
								uint field = key.Field;
								if (field == 0u)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
							else if (instance.ChannelState == null)
							{
								instance.ChannelState = ChannelState.DeserializeLengthDelimited(stream);
							}
							else
							{
								ChannelState.DeserializeLengthDelimited(stream, instance.ChannelState);
							}
						}
						else
						{
							instance.Member.Add(bnet.protocol.channel.Member.DeserializeLengthDelimited(stream));
						}
					}
					else if (instance.Self == null)
					{
						instance.Self = bnet.protocol.channel.Member.DeserializeLengthDelimited(stream);
					}
					else
					{
						bnet.protocol.channel.Member.DeserializeLengthDelimited(stream, instance.Self);
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
			AddNotification.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, AddNotification instance)
		{
			if (instance.HasSelf)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.Self.GetSerializedSize());
				bnet.protocol.channel.Member.Serialize(stream, instance.Self);
			}
			if (instance.Member.get_Count() > 0)
			{
				using (List<Member>.Enumerator enumerator = instance.Member.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Member current = enumerator.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.channel.Member.Serialize(stream, current);
					}
				}
			}
			if (instance.ChannelState == null)
			{
				throw new ArgumentNullException("ChannelState", "Required by proto specification.");
			}
			stream.WriteByte(26);
			ProtocolParser.WriteUInt32(stream, instance.ChannelState.GetSerializedSize());
			ChannelState.Serialize(stream, instance.ChannelState);
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasSelf)
			{
				num += 1u;
				uint serializedSize = this.Self.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.Member.get_Count() > 0)
			{
				using (List<Member>.Enumerator enumerator = this.Member.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Member current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			uint serializedSize3 = this.ChannelState.GetSerializedSize();
			num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
			num += 1u;
			return num;
		}

		public void SetSelf(Member val)
		{
			this.Self = val;
		}

		public void AddMember(Member val)
		{
			this._Member.Add(val);
		}

		public void ClearMember()
		{
			this._Member.Clear();
		}

		public void SetMember(List<Member> val)
		{
			this.Member = val;
		}

		public void SetChannelState(ChannelState val)
		{
			this.ChannelState = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasSelf)
			{
				num ^= this.Self.GetHashCode();
			}
			using (List<Member>.Enumerator enumerator = this.Member.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Member current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			num ^= this.ChannelState.GetHashCode();
			return num;
		}

		public override bool Equals(object obj)
		{
			AddNotification addNotification = obj as AddNotification;
			if (addNotification == null)
			{
				return false;
			}
			if (this.HasSelf != addNotification.HasSelf || (this.HasSelf && !this.Self.Equals(addNotification.Self)))
			{
				return false;
			}
			if (this.Member.get_Count() != addNotification.Member.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Member.get_Count(); i++)
			{
				if (!this.Member.get_Item(i).Equals(addNotification.Member.get_Item(i)))
				{
					return false;
				}
			}
			return this.ChannelState.Equals(addNotification.ChannelState);
		}

		public static AddNotification ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<AddNotification>(bs, 0, -1);
		}
	}
}
