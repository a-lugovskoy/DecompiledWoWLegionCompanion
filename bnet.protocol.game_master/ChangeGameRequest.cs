using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class ChangeGameRequest : IProtoBuf
	{
		public bool HasOpen;

		private bool _Open;

		private List<Attribute> _Attribute = new List<Attribute>();

		public bool HasReplace;

		private bool _Replace;

		public GameHandle GameHandle
		{
			get;
			set;
		}

		public bool Open
		{
			get
			{
				return this._Open;
			}
			set
			{
				this._Open = value;
				this.HasOpen = true;
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

		public bool Replace
		{
			get
			{
				return this._Replace;
			}
			set
			{
				this._Replace = value;
				this.HasReplace = true;
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
			ChangeGameRequest.Deserialize(stream, this);
		}

		public static ChangeGameRequest Deserialize(Stream stream, ChangeGameRequest instance)
		{
			return ChangeGameRequest.Deserialize(stream, instance, -1L);
		}

		public static ChangeGameRequest DeserializeLengthDelimited(Stream stream)
		{
			ChangeGameRequest changeGameRequest = new ChangeGameRequest();
			ChangeGameRequest.DeserializeLengthDelimited(stream, changeGameRequest);
			return changeGameRequest;
		}

		public static ChangeGameRequest DeserializeLengthDelimited(Stream stream, ChangeGameRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ChangeGameRequest.Deserialize(stream, instance, num);
		}

		public static ChangeGameRequest Deserialize(Stream stream, ChangeGameRequest instance, long limit)
		{
			if (instance.Attribute == null)
			{
				instance.Attribute = new List<Attribute>();
			}
			instance.Replace = false;
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
						if (num2 != 16)
						{
							if (num2 != 26)
							{
								if (num2 != 32)
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
									instance.Replace = ProtocolParser.ReadBool(stream);
								}
							}
							else
							{
								instance.Attribute.Add(bnet.protocol.attribute.Attribute.DeserializeLengthDelimited(stream));
							}
						}
						else
						{
							instance.Open = ProtocolParser.ReadBool(stream);
						}
					}
					else if (instance.GameHandle == null)
					{
						instance.GameHandle = GameHandle.DeserializeLengthDelimited(stream);
					}
					else
					{
						GameHandle.DeserializeLengthDelimited(stream, instance.GameHandle);
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
			ChangeGameRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ChangeGameRequest instance)
		{
			if (instance.GameHandle == null)
			{
				throw new ArgumentNullException("GameHandle", "Required by proto specification.");
			}
			stream.WriteByte(10);
			ProtocolParser.WriteUInt32(stream, instance.GameHandle.GetSerializedSize());
			GameHandle.Serialize(stream, instance.GameHandle);
			if (instance.HasOpen)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteBool(stream, instance.Open);
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
			if (instance.HasReplace)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteBool(stream, instance.Replace);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			uint serializedSize = this.GameHandle.GetSerializedSize();
			num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			if (this.HasOpen)
			{
				num += 1u;
				num += 1u;
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
			if (this.HasReplace)
			{
				num += 1u;
				num += 1u;
			}
			num += 1u;
			return num;
		}

		public void SetGameHandle(GameHandle val)
		{
			this.GameHandle = val;
		}

		public void SetOpen(bool val)
		{
			this.Open = val;
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

		public void SetReplace(bool val)
		{
			this.Replace = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.GameHandle.GetHashCode();
			if (this.HasOpen)
			{
				num ^= this.Open.GetHashCode();
			}
			using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			if (this.HasReplace)
			{
				num ^= this.Replace.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ChangeGameRequest changeGameRequest = obj as ChangeGameRequest;
			if (changeGameRequest == null)
			{
				return false;
			}
			if (!this.GameHandle.Equals(changeGameRequest.GameHandle))
			{
				return false;
			}
			if (this.HasOpen != changeGameRequest.HasOpen || (this.HasOpen && !this.Open.Equals(changeGameRequest.Open)))
			{
				return false;
			}
			if (this.Attribute.get_Count() != changeGameRequest.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(changeGameRequest.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasReplace == changeGameRequest.HasReplace && (!this.HasReplace || this.Replace.Equals(changeGameRequest.Replace));
		}

		public static ChangeGameRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ChangeGameRequest>(bs, 0, -1);
		}
	}
}
