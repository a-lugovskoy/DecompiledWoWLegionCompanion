using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_utilities
{
	public class ServerRequest : IProtoBuf
	{
		private List<Attribute> _Attribute = new List<Attribute>();

		public bool HasHost;

		private ProcessId _Host;

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

		public uint Program
		{
			get;
			set;
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

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			ServerRequest.Deserialize(stream, this);
		}

		public static ServerRequest Deserialize(Stream stream, ServerRequest instance)
		{
			return ServerRequest.Deserialize(stream, instance, -1L);
		}

		public static ServerRequest DeserializeLengthDelimited(Stream stream)
		{
			ServerRequest serverRequest = new ServerRequest();
			ServerRequest.DeserializeLengthDelimited(stream, serverRequest);
			return serverRequest;
		}

		public static ServerRequest DeserializeLengthDelimited(Stream stream, ServerRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ServerRequest.Deserialize(stream, instance, num);
		}

		public static ServerRequest Deserialize(Stream stream, ServerRequest instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
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
						if (num2 != 21)
						{
							if (num2 != 26)
							{
								Key key = ProtocolParser.ReadKey((byte)num, stream);
								uint field = key.Field;
								if (field == 0u)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
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
							instance.Program = binaryReader.ReadUInt32();
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
			ServerRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ServerRequest instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
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
			stream.WriteByte(21);
			binaryWriter.Write(instance.Program);
			if (instance.HasHost)
			{
				stream.WriteByte(26);
				ProtocolParser.WriteUInt32(stream, instance.Host.GetSerializedSize());
				ProcessId.Serialize(stream, instance.Host);
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
			num += 4u;
			if (this.HasHost)
			{
				num += 1u;
				uint serializedSize2 = this.Host.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
			num += 1u;
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

		public void SetProgram(uint val)
		{
			this.Program = val;
		}

		public void SetHost(ProcessId val)
		{
			this.Host = val;
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
			num ^= this.Program.GetHashCode();
			if (this.HasHost)
			{
				num ^= this.Host.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ServerRequest serverRequest = obj as ServerRequest;
			if (serverRequest == null)
			{
				return false;
			}
			if (this.Attribute.get_Count() != serverRequest.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(serverRequest.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			return this.Program.Equals(serverRequest.Program) && this.HasHost == serverRequest.HasHost && (!this.HasHost || this.Host.Equals(serverRequest.Host));
		}

		public static ServerRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ServerRequest>(bs, 0, -1);
		}
	}
}
