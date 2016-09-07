using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel_invitation
{
	public class IncrementChannelCountResponse : IProtoBuf
	{
		private List<ulong> _ReservationTokens = new List<ulong>();

		public List<ulong> ReservationTokens
		{
			get
			{
				return this._ReservationTokens;
			}
			set
			{
				this._ReservationTokens = value;
			}
		}

		public List<ulong> ReservationTokensList
		{
			get
			{
				return this._ReservationTokens;
			}
		}

		public int ReservationTokensCount
		{
			get
			{
				return this._ReservationTokens.get_Count();
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
			IncrementChannelCountResponse.Deserialize(stream, this);
		}

		public static IncrementChannelCountResponse Deserialize(Stream stream, IncrementChannelCountResponse instance)
		{
			return IncrementChannelCountResponse.Deserialize(stream, instance, -1L);
		}

		public static IncrementChannelCountResponse DeserializeLengthDelimited(Stream stream)
		{
			IncrementChannelCountResponse incrementChannelCountResponse = new IncrementChannelCountResponse();
			IncrementChannelCountResponse.DeserializeLengthDelimited(stream, incrementChannelCountResponse);
			return incrementChannelCountResponse;
		}

		public static IncrementChannelCountResponse DeserializeLengthDelimited(Stream stream, IncrementChannelCountResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return IncrementChannelCountResponse.Deserialize(stream, instance, num);
		}

		public static IncrementChannelCountResponse Deserialize(Stream stream, IncrementChannelCountResponse instance, long limit)
		{
			if (instance.ReservationTokens == null)
			{
				instance.ReservationTokens = new List<ulong>();
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
					if (num2 != 8)
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
						instance.ReservationTokens.Add(ProtocolParser.ReadUInt64(stream));
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
			IncrementChannelCountResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, IncrementChannelCountResponse instance)
		{
			if (instance.ReservationTokens.get_Count() > 0)
			{
				using (List<ulong>.Enumerator enumerator = instance.ReservationTokens.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ulong current = enumerator.get_Current();
						stream.WriteByte(8);
						ProtocolParser.WriteUInt64(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.ReservationTokens.get_Count() > 0)
			{
				using (List<ulong>.Enumerator enumerator = this.ReservationTokens.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ulong current = enumerator.get_Current();
						num += 1u;
						num += ProtocolParser.SizeOfUInt64(current);
					}
				}
			}
			return num;
		}

		public void AddReservationTokens(ulong val)
		{
			this._ReservationTokens.Add(val);
		}

		public void ClearReservationTokens()
		{
			this._ReservationTokens.Clear();
		}

		public void SetReservationTokens(List<ulong> val)
		{
			this.ReservationTokens = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<ulong>.Enumerator enumerator = this.ReservationTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ulong current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			IncrementChannelCountResponse incrementChannelCountResponse = obj as IncrementChannelCountResponse;
			if (incrementChannelCountResponse == null)
			{
				return false;
			}
			if (this.ReservationTokens.get_Count() != incrementChannelCountResponse.ReservationTokens.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.ReservationTokens.get_Count(); i++)
			{
				if (!this.ReservationTokens.get_Item(i).Equals(incrementChannelCountResponse.ReservationTokens.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static IncrementChannelCountResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<IncrementChannelCountResponse>(bs, 0, -1);
		}
	}
}
