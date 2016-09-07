using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel
{
	public class FindChannelResponse : IProtoBuf
	{
		private List<ChannelDescription> _Channel = new List<ChannelDescription>();

		public List<ChannelDescription> Channel
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

		public List<ChannelDescription> ChannelList
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
			FindChannelResponse.Deserialize(stream, this);
		}

		public static FindChannelResponse Deserialize(Stream stream, FindChannelResponse instance)
		{
			return FindChannelResponse.Deserialize(stream, instance, -1L);
		}

		public static FindChannelResponse DeserializeLengthDelimited(Stream stream)
		{
			FindChannelResponse findChannelResponse = new FindChannelResponse();
			FindChannelResponse.DeserializeLengthDelimited(stream, findChannelResponse);
			return findChannelResponse;
		}

		public static FindChannelResponse DeserializeLengthDelimited(Stream stream, FindChannelResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return FindChannelResponse.Deserialize(stream, instance, num);
		}

		public static FindChannelResponse Deserialize(Stream stream, FindChannelResponse instance, long limit)
		{
			if (instance.Channel == null)
			{
				instance.Channel = new List<ChannelDescription>();
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
						instance.Channel.Add(ChannelDescription.DeserializeLengthDelimited(stream));
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
			FindChannelResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, FindChannelResponse instance)
		{
			if (instance.Channel.get_Count() > 0)
			{
				using (List<ChannelDescription>.Enumerator enumerator = instance.Channel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChannelDescription current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						ChannelDescription.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Channel.get_Count() > 0)
			{
				using (List<ChannelDescription>.Enumerator enumerator = this.Channel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChannelDescription current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddChannel(ChannelDescription val)
		{
			this._Channel.Add(val);
		}

		public void ClearChannel()
		{
			this._Channel.Clear();
		}

		public void SetChannel(List<ChannelDescription> val)
		{
			this.Channel = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<ChannelDescription>.Enumerator enumerator = this.Channel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChannelDescription current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			FindChannelResponse findChannelResponse = obj as FindChannelResponse;
			if (findChannelResponse == null)
			{
				return false;
			}
			if (this.Channel.get_Count() != findChannelResponse.Channel.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Channel.get_Count(); i++)
			{
				if (!this.Channel.get_Item(i).Equals(findChannelResponse.Channel.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static FindChannelResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<FindChannelResponse>(bs, 0, -1);
		}
	}
}
