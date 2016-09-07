using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel_invitation
{
	public class ListChannelCountResponse : IProtoBuf
	{
		private List<ChannelCount> _Channel = new List<ChannelCount>();

		public List<ChannelCount> Channel
		{
			get
			{
				return this._Channel;
			}
			set
			{
				this._Channel = value;
			}
		}

		public List<ChannelCount> ChannelList
		{
			get
			{
				return this._Channel;
			}
		}

		public int ChannelCount
		{
			get
			{
				return this._Channel.get_Count();
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
			ListChannelCountResponse.Deserialize(stream, this);
		}

		public static ListChannelCountResponse Deserialize(Stream stream, ListChannelCountResponse instance)
		{
			return ListChannelCountResponse.Deserialize(stream, instance, -1L);
		}

		public static ListChannelCountResponse DeserializeLengthDelimited(Stream stream)
		{
			ListChannelCountResponse listChannelCountResponse = new ListChannelCountResponse();
			ListChannelCountResponse.DeserializeLengthDelimited(stream, listChannelCountResponse);
			return listChannelCountResponse;
		}

		public static ListChannelCountResponse DeserializeLengthDelimited(Stream stream, ListChannelCountResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ListChannelCountResponse.Deserialize(stream, instance, num);
		}

		public static ListChannelCountResponse Deserialize(Stream stream, ListChannelCountResponse instance, long limit)
		{
			if (instance.Channel == null)
			{
				instance.Channel = new List<ChannelCount>();
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
						instance.Channel.Add(bnet.protocol.channel_invitation.ChannelCount.DeserializeLengthDelimited(stream));
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
			ListChannelCountResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ListChannelCountResponse instance)
		{
			if (instance.Channel.get_Count() > 0)
			{
				using (List<ChannelCount>.Enumerator enumerator = instance.Channel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChannelCount current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.channel_invitation.ChannelCount.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Channel.get_Count() > 0)
			{
				using (List<ChannelCount>.Enumerator enumerator = this.Channel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChannelCount current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddChannel(ChannelCount val)
		{
			this._Channel.Add(val);
		}

		public void ClearChannel()
		{
			this._Channel.Clear();
		}

		public void SetChannel(List<ChannelCount> val)
		{
			this.Channel = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<ChannelCount>.Enumerator enumerator = this.Channel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChannelCount current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ListChannelCountResponse listChannelCountResponse = obj as ListChannelCountResponse;
			if (listChannelCountResponse == null)
			{
				return false;
			}
			if (this.Channel.get_Count() != listChannelCountResponse.Channel.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Channel.get_Count(); i++)
			{
				if (!this.Channel.get_Item(i).Equals(listChannelCountResponse.Channel.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static ListChannelCountResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ListChannelCountResponse>(bs, 0, -1);
		}
	}
}
