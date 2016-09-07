using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol.game_master
{
	public class GameFactoryDescription : IProtoBuf
	{
		public bool HasName;

		private string _Name;

		private List<Attribute> _Attribute = new List<Attribute>();

		private List<GameStatsBucket> _StatsBucket = new List<GameStatsBucket>();

		public bool HasUnseededId;

		private ulong _UnseededId;

		public bool HasAllowQueueing;

		private bool _AllowQueueing;

		public ulong Id
		{
			get;
			set;
		}

		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.HasName = (value != null);
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

		public ulong UnseededId
		{
			get
			{
				return this._UnseededId;
			}
			set
			{
				this._UnseededId = value;
				this.HasUnseededId = true;
			}
		}

		public bool AllowQueueing
		{
			get
			{
				return this._AllowQueueing;
			}
			set
			{
				this._AllowQueueing = value;
				this.HasAllowQueueing = true;
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
			GameFactoryDescription.Deserialize(stream, this);
		}

		public static GameFactoryDescription Deserialize(Stream stream, GameFactoryDescription instance)
		{
			return GameFactoryDescription.Deserialize(stream, instance, -1L);
		}

		public static GameFactoryDescription DeserializeLengthDelimited(Stream stream)
		{
			GameFactoryDescription gameFactoryDescription = new GameFactoryDescription();
			GameFactoryDescription.DeserializeLengthDelimited(stream, gameFactoryDescription);
			return gameFactoryDescription;
		}

		public static GameFactoryDescription DeserializeLengthDelimited(Stream stream, GameFactoryDescription instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GameFactoryDescription.Deserialize(stream, instance, num);
		}

		public static GameFactoryDescription Deserialize(Stream stream, GameFactoryDescription instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.Attribute == null)
			{
				instance.Attribute = new List<Attribute>();
			}
			if (instance.StatsBucket == null)
			{
				instance.StatsBucket = new List<GameStatsBucket>();
			}
			instance.UnseededId = 0uL;
			instance.AllowQueueing = true;
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
					if (num2 != 9)
					{
						if (num2 != 18)
						{
							if (num2 != 26)
							{
								if (num2 != 34)
								{
									if (num2 != 41)
									{
										if (num2 != 48)
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
											instance.AllowQueueing = ProtocolParser.ReadBool(stream);
										}
									}
									else
									{
										instance.UnseededId = binaryReader.ReadUInt64();
									}
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
						else
						{
							instance.Name = ProtocolParser.ReadString(stream);
						}
					}
					else
					{
						instance.Id = binaryReader.ReadUInt64();
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
			GameFactoryDescription.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GameFactoryDescription instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			stream.WriteByte(9);
			binaryWriter.Write(instance.Id);
			if (instance.HasName)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Name));
			}
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = instance.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						stream.WriteByte(26);
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
						stream.WriteByte(34);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						GameStatsBucket.Serialize(stream, current2);
					}
				}
			}
			if (instance.HasUnseededId)
			{
				stream.WriteByte(41);
				binaryWriter.Write(instance.UnseededId);
			}
			if (instance.HasAllowQueueing)
			{
				stream.WriteByte(48);
				ProtocolParser.WriteBool(stream, instance.AllowQueueing);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += 8u;
			if (this.HasName)
			{
				num += 1u;
				uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.Name);
				num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			}
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
			if (this.HasUnseededId)
			{
				num += 1u;
				num += 8u;
			}
			if (this.HasAllowQueueing)
			{
				num += 1u;
				num += 1u;
			}
			num += 1u;
			return num;
		}

		public void SetId(ulong val)
		{
			this.Id = val;
		}

		public void SetName(string val)
		{
			this.Name = val;
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

		public void SetUnseededId(ulong val)
		{
			this.UnseededId = val;
		}

		public void SetAllowQueueing(bool val)
		{
			this.AllowQueueing = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.Id.GetHashCode();
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
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
			if (this.HasUnseededId)
			{
				num ^= this.UnseededId.GetHashCode();
			}
			if (this.HasAllowQueueing)
			{
				num ^= this.AllowQueueing.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GameFactoryDescription gameFactoryDescription = obj as GameFactoryDescription;
			if (gameFactoryDescription == null)
			{
				return false;
			}
			if (!this.Id.Equals(gameFactoryDescription.Id))
			{
				return false;
			}
			if (this.HasName != gameFactoryDescription.HasName || (this.HasName && !this.Name.Equals(gameFactoryDescription.Name)))
			{
				return false;
			}
			if (this.Attribute.get_Count() != gameFactoryDescription.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(gameFactoryDescription.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			if (this.StatsBucket.get_Count() != gameFactoryDescription.StatsBucket.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.StatsBucket.get_Count(); j++)
			{
				if (!this.StatsBucket.get_Item(j).Equals(gameFactoryDescription.StatsBucket.get_Item(j)))
				{
					return false;
				}
			}
			return this.HasUnseededId == gameFactoryDescription.HasUnseededId && (!this.HasUnseededId || this.UnseededId.Equals(gameFactoryDescription.UnseededId)) && this.HasAllowQueueing == gameFactoryDescription.HasAllowQueueing && (!this.HasAllowQueueing || this.AllowQueueing.Equals(gameFactoryDescription.AllowQueueing));
		}

		public static GameFactoryDescription ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GameFactoryDescription>(bs, 0, -1);
		}
	}
}
