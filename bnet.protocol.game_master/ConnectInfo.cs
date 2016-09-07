using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol.game_master
{
	public class ConnectInfo : IProtoBuf
	{
		public bool HasToken;

		private byte[] _Token;

		private List<Attribute> _Attribute = new List<Attribute>();

		public EntityId MemberId
		{
			get;
			set;
		}

		public string Host
		{
			get;
			set;
		}

		public int Port
		{
			get;
			set;
		}

		public byte[] Token
		{
			get
			{
				return this._Token;
			}
			set
			{
				this._Token = value;
				this.HasToken = (value != null);
			}
		}

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

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			ConnectInfo.Deserialize(stream, this);
		}

		public static ConnectInfo Deserialize(Stream stream, ConnectInfo instance)
		{
			return ConnectInfo.Deserialize(stream, instance, -1L);
		}

		public static ConnectInfo DeserializeLengthDelimited(Stream stream)
		{
			ConnectInfo connectInfo = new ConnectInfo();
			ConnectInfo.DeserializeLengthDelimited(stream, connectInfo);
			return connectInfo;
		}

		public static ConnectInfo DeserializeLengthDelimited(Stream stream, ConnectInfo instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ConnectInfo.Deserialize(stream, instance, num);
		}

		public static ConnectInfo Deserialize(Stream stream, ConnectInfo instance, long limit)
		{
			if (instance.Attribute == null)
			{
				instance.Attribute = new List<Attribute>();
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
							if (num2 != 24)
							{
								if (num2 != 34)
								{
									if (num2 != 42)
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
										instance.Attribute.Add(bnet.protocol.attribute.Attribute.DeserializeLengthDelimited(stream));
									}
								}
								else
								{
									instance.Token = ProtocolParser.ReadBytes(stream);
								}
							}
							else
							{
								instance.Port = (int)ProtocolParser.ReadUInt64(stream);
							}
						}
						else
						{
							instance.Host = ProtocolParser.ReadString(stream);
						}
					}
					else if (instance.MemberId == null)
					{
						instance.MemberId = EntityId.DeserializeLengthDelimited(stream);
					}
					else
					{
						EntityId.DeserializeLengthDelimited(stream, instance.MemberId);
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
			ConnectInfo.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ConnectInfo instance)
		{
			if (instance.MemberId == null)
			{
				throw new ArgumentNullException("MemberId", "Required by proto specification.");
			}
			stream.WriteByte(10);
			ProtocolParser.WriteUInt32(stream, instance.MemberId.GetSerializedSize());
			EntityId.Serialize(stream, instance.MemberId);
			if (instance.Host == null)
			{
				throw new ArgumentNullException("Host", "Required by proto specification.");
			}
			stream.WriteByte(18);
			ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Host));
			stream.WriteByte(24);
			ProtocolParser.WriteUInt64(stream, (ulong)((long)instance.Port));
			if (instance.HasToken)
			{
				stream.WriteByte(34);
				ProtocolParser.WriteBytes(stream, instance.Token);
			}
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = instance.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						stream.WriteByte(42);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.attribute.Attribute.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			uint serializedSize = this.MemberId.GetSerializedSize();
			num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.Host);
			num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			num += ProtocolParser.SizeOfUInt64((ulong)((long)this.Port));
			if (this.HasToken)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.Token.Length) + (uint)this.Token.Length;
			}
			if (this.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			num += 3u;
			return num;
		}

		public void SetMemberId(EntityId val)
		{
			this.MemberId = val;
		}

		public void SetHost(string val)
		{
			this.Host = val;
		}

		public void SetPort(int val)
		{
			this.Port = val;
		}

		public void SetToken(byte[] val)
		{
			this.Token = val;
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

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.MemberId.GetHashCode();
			num ^= this.Host.GetHashCode();
			num ^= this.Port.GetHashCode();
			if (this.HasToken)
			{
				num ^= this.Token.GetHashCode();
			}
			using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ConnectInfo connectInfo = obj as ConnectInfo;
			if (connectInfo == null)
			{
				return false;
			}
			if (!this.MemberId.Equals(connectInfo.MemberId))
			{
				return false;
			}
			if (!this.Host.Equals(connectInfo.Host))
			{
				return false;
			}
			if (!this.Port.Equals(connectInfo.Port))
			{
				return false;
			}
			if (this.HasToken != connectInfo.HasToken || (this.HasToken && !this.Token.Equals(connectInfo.Token)))
			{
				return false;
			}
			if (this.Attribute.get_Count() != connectInfo.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(connectInfo.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static ConnectInfo ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ConnectInfo>(bs, 0, -1);
		}
	}
}
