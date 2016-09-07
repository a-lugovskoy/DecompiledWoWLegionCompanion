using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class GetGameStatsResponse : IProtoBuf
	{
		private List<GameStatsBucket> _StatsBucket = new List<GameStatsBucket>();

		public List<GameStatsBucket> StatsBucket
		{
			get
			{
				return this._StatsBucket;
			}
			set
			{
				this._StatsBucket = value;
			}
		}

		public List<GameStatsBucket> StatsBucketList
		{
			get
			{
				return this._StatsBucket;
			}
		}

		public int StatsBucketCount
		{
			get
			{
				return this._StatsBucket.get_Count();
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
			GetGameStatsResponse.Deserialize(stream, this);
		}

		public static GetGameStatsResponse Deserialize(Stream stream, GetGameStatsResponse instance)
		{
			return GetGameStatsResponse.Deserialize(stream, instance, -1L);
		}

		public static GetGameStatsResponse DeserializeLengthDelimited(Stream stream)
		{
			GetGameStatsResponse getGameStatsResponse = new GetGameStatsResponse();
			GetGameStatsResponse.DeserializeLengthDelimited(stream, getGameStatsResponse);
			return getGameStatsResponse;
		}

		public static GetGameStatsResponse DeserializeLengthDelimited(Stream stream, GetGameStatsResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GetGameStatsResponse.Deserialize(stream, instance, num);
		}

		public static GetGameStatsResponse Deserialize(Stream stream, GetGameStatsResponse instance, long limit)
		{
			if (instance.StatsBucket == null)
			{
				instance.StatsBucket = new List<GameStatsBucket>();
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
						instance.StatsBucket.Add(GameStatsBucket.DeserializeLengthDelimited(stream));
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
			GetGameStatsResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GetGameStatsResponse instance)
		{
			if (instance.StatsBucket.get_Count() > 0)
			{
				using (List<GameStatsBucket>.Enumerator enumerator = instance.StatsBucket.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameStatsBucket current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						GameStatsBucket.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.StatsBucket.get_Count() > 0)
			{
				using (List<GameStatsBucket>.Enumerator enumerator = this.StatsBucket.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameStatsBucket current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddStatsBucket(GameStatsBucket val)
		{
			this._StatsBucket.Add(val);
		}

		public void ClearStatsBucket()
		{
			this._StatsBucket.Clear();
		}

		public void SetStatsBucket(List<GameStatsBucket> val)
		{
			this.StatsBucket = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<GameStatsBucket>.Enumerator enumerator = this.StatsBucket.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameStatsBucket current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GetGameStatsResponse getGameStatsResponse = obj as GetGameStatsResponse;
			if (getGameStatsResponse == null)
			{
				return false;
			}
			if (this.StatsBucket.get_Count() != getGameStatsResponse.StatsBucket.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.StatsBucket.get_Count(); i++)
			{
				if (!this.StatsBucket.get_Item(i).Equals(getGameStatsResponse.StatsBucket.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static GetGameStatsResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GetGameStatsResponse>(bs, 0, -1);
		}
	}
}
