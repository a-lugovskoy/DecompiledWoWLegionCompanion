using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel
{
	public class Message : IProtoBuf
	{
		private List<Attribute> _Attribute = new List<Attribute>();

		public bool HasRole;

		private uint _Role;

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

		public uint Role
		{
			get
			{
				return this._Role;
			}
			set
			{
				this._Role = value;
				this.HasRole = true;
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
			Message.Deserialize(stream, this);
		}

		public static Message Deserialize(Stream stream, Message instance)
		{
			return Message.Deserialize(stream, instance, -1L);
		}

		public static Message DeserializeLengthDelimited(Stream stream)
		{
			Message message = new Message();
			Message.DeserializeLengthDelimited(stream, message);
			return message;
		}

		public static Message DeserializeLengthDelimited(Stream stream, Message instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return Message.Deserialize(stream, instance, num);
		}

		public static Message Deserialize(Stream stream, Message instance, long limit)
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
						if (num2 != 16)
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
							instance.Role = ProtocolParser.ReadUInt32(stream);
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
			Message.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, Message instance)
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
			if (instance.HasRole)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, instance.Role);
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
			if (this.HasRole)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.Role);
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

		public void SetRole(uint val)
		{
			this.Role = val;
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
			if (this.HasRole)
			{
				num ^= this.Role.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			Message message = obj as Message;
			if (message == null)
			{
				return false;
			}
			if (this.Attribute.get_Count() != message.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(message.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasRole == message.HasRole && (!this.HasRole || this.Role.Equals(message.Role));
		}

		public static Message ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<Message>(bs, 0, -1);
		}
	}
}
