using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_utilities
{
	public class ClientRequest : IProtoBuf
	{
		private List<Attribute> _Attribute = new List<Attribute>();

		public bool HasHost;

		private ProcessId _Host;

		public bool HasBnetAccountId;

		private EntityId _BnetAccountId;

		public bool HasGameAccountId;

		private EntityId _GameAccountId;

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

		public ProcessId Host
		{
			get
			{
				return this._Host;
			}
			set
			{
				this._Host = value;
				this.HasHost = (value != null);
			}
		}

		public EntityId BnetAccountId
		{
			get
			{
				return this._BnetAccountId;
			}
			set
			{
				this._BnetAccountId = value;
				this.HasBnetAccountId = (value != null);
			}
		}

		public EntityId GameAccountId
		{
			get
			{
				return this._GameAccountId;
			}
			set
			{
				this._GameAccountId = value;
				this.HasGameAccountId = (value != null);
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
			ClientRequest.Deserialize(stream, this);
		}

		public static ClientRequest Deserialize(Stream stream, ClientRequest instance)
		{
			return ClientRequest.Deserialize(stream, instance, -1L);
		}

		public static ClientRequest DeserializeLengthDelimited(Stream stream)
		{
			ClientRequest clientRequest = new ClientRequest();
			ClientRequest.DeserializeLengthDelimited(stream, clientRequest);
			return clientRequest;
		}

		public static ClientRequest DeserializeLengthDelimited(Stream stream, ClientRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ClientRequest.Deserialize(stream, instance, num);
		}

		public static ClientRequest Deserialize(Stream stream, ClientRequest instance, long limit)
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
							if (num2 != 26)
							{
								if (num2 != 34)
								{
									Key key = ProtocolParser.ReadKey((byte)num, stream);
									uint field = key.Field;
									if (field == 0u)
									{
										throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
									}
									ProtocolParser.SkipKey(stream, key);
								}
								else if (instance.GameAccountId == null)
								{
									instance.GameAccountId = EntityId.DeserializeLengthDelimited(stream);
								}
								else
								{
									EntityId.DeserializeLengthDelimited(stream, instance.GameAccountId);
								}
							}
							else if (instance.BnetAccountId == null)
							{
								instance.BnetAccountId = EntityId.DeserializeLengthDelimited(stream);
							}
							else
							{
								EntityId.DeserializeLengthDelimited(stream, instance.BnetAccountId);
							}
						}
						else if (instance.Host == null)
						{
							instance.Host = ProcessId.DeserializeLengthDelimited(stream);
						}
						else
						{
							ProcessId.DeserializeLengthDelimited(stream, instance.Host);
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
			ClientRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ClientRequest instance)
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
			if (instance.HasHost)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteUInt32(stream, instance.Host.GetSerializedSize());
				ProcessId.Serialize(stream, instance.Host);
			}
			if (instance.HasBnetAccountId)
			{
				stream.WriteByte(26);
				ProtocolParser.WriteUInt32(stream, instance.BnetAccountId.GetSerializedSize());
				EntityId.Serialize(stream, instance.BnetAccountId);
			}
			if (instance.HasGameAccountId)
			{
				stream.WriteByte(34);
				ProtocolParser.WriteUInt32(stream, instance.GameAccountId.GetSerializedSize());
				EntityId.Serialize(stream, instance.GameAccountId);
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
			if (this.HasHost)
			{
				num += 1u;
				uint serializedSize2 = this.Host.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
			if (this.HasBnetAccountId)
			{
				num += 1u;
				uint serializedSize3 = this.BnetAccountId.GetSerializedSize();
				num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
			}
			if (this.HasGameAccountId)
			{
				num += 1u;
				uint serializedSize4 = this.GameAccountId.GetSerializedSize();
				num += serializedSize4 + ProtocolParser.SizeOfUInt32(serializedSize4);
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

		public void SetHost(ProcessId val)
		{
			this.Host = val;
		}

		public void SetBnetAccountId(EntityId val)
		{
			this.BnetAccountId = val;
		}

		public void SetGameAccountId(EntityId val)
		{
			this.GameAccountId = val;
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
			if (this.HasHost)
			{
				num ^= this.Host.GetHashCode();
			}
			if (this.HasBnetAccountId)
			{
				num ^= this.BnetAccountId.GetHashCode();
			}
			if (this.HasGameAccountId)
			{
				num ^= this.GameAccountId.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ClientRequest clientRequest = obj as ClientRequest;
			if (clientRequest == null)
			{
				return false;
			}
			if (this.Attribute.get_Count() != clientRequest.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(clientRequest.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasHost == clientRequest.HasHost && (!this.HasHost || this.Host.Equals(clientRequest.Host)) && this.HasBnetAccountId == clientRequest.HasBnetAccountId && (!this.HasBnetAccountId || this.BnetAccountId.Equals(clientRequest.BnetAccountId)) && this.HasGameAccountId == clientRequest.HasGameAccountId && (!this.HasGameAccountId || this.GameAccountId.Equals(clientRequest.GameAccountId));
		}

		public static ClientRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ClientRequest>(bs, 0, -1);
		}
	}
}
