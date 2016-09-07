using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class GetFactoryInfoResponse : IProtoBuf
	{
		private List<Attribute> _Attribute = new List<Attribute>();

		private List<GameStatsBucket> _StatsBucket = new List<GameStatsBucket>();

		public List<Attribute> Attribute
		{
			get
			{
				return this._Attribute;
			}
			set
			{
				this._Attribute = value;
			}
		}

		public List<Attribute> AttributeList
		{
			get
			{
				return this._Attribute;
			}
		}

		public int AttributeCount
		{
			get
			{
				return this._Attribute.get_Count();
			}
		}

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
			GetFactoryInfoResponse.Deserialize(stream, this);
		}

		public static GetFactoryInfoResponse Deserialize(Stream stream, GetFactoryInfoResponse instance)
		{
			return GetFactoryInfoResponse.Deserialize(stream, instance, -1L);
		}

		public static GetFactoryInfoResponse DeserializeLengthDelimited(Stream stream)
		{
			GetFactoryInfoResponse getFactoryInfoResponse = new GetFactoryInfoResponse();
			GetFactoryInfoResponse.DeserializeLengthDelimited(stream, getFactoryInfoResponse);
			return getFactoryInfoResponse;
		}

		public static GetFactoryInfoResponse DeserializeLengthDelimited(Stream stream, GetFactoryInfoResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GetFactoryInfoResponse.Deserialize(stream, instance, num);
		}

		public static GetFactoryInfoResponse Deserialize(Stream stream, GetFactoryInfoResponse instance, long limit)
		{
			if (instance.Attribute == null)
			{
				instance.Attribute = new List<Attribute>();
			}
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
							instance.StatsBucket.Add(GameStatsBucket.DeserializeLengthDelimited(stream));
						}
					}
					else
					{
						instance.Attribute.Add(bnet.protocol.attribute.Attribute.DeserializeLengthDelimited(stream));
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
			GetFactoryInfoResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GetFactoryInfoResponse instance)
		{
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = instance.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.attribute.Attribute.Serialize(stream, current);
					}
				}
			}
			if (instance.StatsBucket.get_Count() > 0)
			{
				using (List<GameStatsBucket>.Enumerator enumerator2 = instance.StatsBucket.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameStatsBucket current2 = enumerator2.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						GameStatsBucket.Serialize(stream, current2);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.StatsBucket.get_Count() > 0)
			{
				using (List<GameStatsBucket>.Enumerator enumerator2 = this.StatsBucket.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameStatsBucket current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize2 = current2.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			return num;
		}

		public void AddAttribute(Attribute val)
		{
			this._Attribute.Add(val);
		}

		public void ClearAttribute()
		{
			this._Attribute.Clear();
		}

		public void SetAttribute(List<Attribute> val)
		{
			this.Attribute = val;
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
			using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<GameStatsBucket>.Enumerator enumerator2 = this.StatsBucket.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameStatsBucket current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GetFactoryInfoResponse getFactoryInfoResponse = obj as GetFactoryInfoResponse;
			if (getFactoryInfoResponse == null)
			{
				return false;
			}
			if (this.Attribute.get_Count() != getFactoryInfoResponse.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(getFactoryInfoResponse.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			if (this.StatsBucket.get_Count() != getFactoryInfoResponse.StatsBucket.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.StatsBucket.get_Count(); j++)
			{
				if (!this.StatsBucket.get_Item(j).Equals(getFactoryInfoResponse.StatsBucket.get_Item(j)))
				{
					return false;
				}
			}
			return true;
		}

		public static GetFactoryInfoResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GetFactoryInfoResponse>(bs, 0, -1);
		}
	}
}
