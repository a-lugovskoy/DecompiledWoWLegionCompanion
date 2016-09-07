using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel
{
	public class ChannelInfo : IProtoBuf
	{
		private List<Member> _Member = new List<Member>();

		public ChannelDescription Description
		{
			get;
			set;
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

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			ChannelInfo.Deserialize(stream, this);
		}

		public static ChannelInfo Deserialize(Stream stream, ChannelInfo instance)
		{
			return ChannelInfo.Deserialize(stream, instance, -1L);
		}

		public static ChannelInfo DeserializeLengthDelimited(Stream stream)
		{
			ChannelInfo channelInfo = new ChannelInfo();
			ChannelInfo.DeserializeLengthDelimited(stream, channelInfo);
			return channelInfo;
		}

		public static ChannelInfo DeserializeLengthDelimited(Stream stream, ChannelInfo instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ChannelInfo.Deserialize(stream, instance, num);
		}

		public static ChannelInfo Deserialize(Stream stream, ChannelInfo instance, long limit)
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
							instance.Member.Add(bnet.protocol.channel.Member.DeserializeLengthDelimited(stream));
						}
					}
					else if (instance.Description == null)
					{
						instance.Description = ChannelDescription.DeserializeLengthDelimited(stream);
					}
					else
					{
						ChannelDescription.DeserializeLengthDelimited(stream, instance.Description);
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
			ChannelInfo.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ChannelInfo instance)
		{
			if (instance.Description == null)
			{
				throw new ArgumentNullException("Description", "Required by proto specification.");
			}
			stream.WriteByte(10);
			ProtocolParser.WriteUInt32(stream, instance.Description.GetSerializedSize());
			ChannelDescription.Serialize(stream, instance.Description);
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
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			uint serializedSize = this.Description.GetSerializedSize();
			num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
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
			num += 1u;
			return num;
		}

		public void SetDescription(ChannelDescription val)
		{
			this.Description = val;
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

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.Description.GetHashCode();
			using (List<Member>.Enumerator enumerator = this.Member.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Member current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ChannelInfo channelInfo = obj as ChannelInfo;
			if (channelInfo == null)
			{
				return false;
			}
			if (!this.Description.Equals(channelInfo.Description))
			{
				return false;
			}
			if (this.Member.get_Count() != channelInfo.Member.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Member.get_Count(); i++)
			{
				if (!this.Member.get_Item(i).Equals(channelInfo.Member.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static ChannelInfo ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ChannelInfo>(bs, 0, -1);
		}
	}
}
