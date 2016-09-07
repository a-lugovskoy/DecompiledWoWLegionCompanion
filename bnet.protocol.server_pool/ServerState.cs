using System;
using System.IO;

namespace bnet.protocol.server_pool
{
	public class ServerState : IProtoBuf
	{
		public bool HasCurrentLoad;

		private float _CurrentLoad;

		public bool HasGameCount;

		private uint _GameCount;

		public bool HasPlayerCount;

		private uint _PlayerCount;

		public float CurrentLoad
		{
			get
			{
				return this._CurrentLoad;
			}
			set
			{
				this._CurrentLoad = value;
				this.HasCurrentLoad = true;
			}
		}

		public uint GameCount
		{
			get
			{
				return this._GameCount;
			}
			set
			{
				this._GameCount = value;
				this.HasGameCount = true;
			}
		}

		public uint PlayerCount
		{
			get
			{
				return this._PlayerCount;
			}
			set
			{
				this._PlayerCount = value;
				this.HasPlayerCount = true;
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
			ServerState.Deserialize(stream, this);
		}

		public static ServerState Deserialize(Stream stream, ServerState instance)
		{
			return ServerState.Deserialize(stream, instance, -1L);
		}

		public static ServerState DeserializeLengthDelimited(Stream stream)
		{
			ServerState serverState = new ServerState();
			ServerState.DeserializeLengthDelimited(stream, serverState);
			return serverState;
		}

		public static ServerState DeserializeLengthDelimited(Stream stream, ServerState instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ServerState.Deserialize(stream, instance, num);
		}

		public static ServerState Deserialize(Stream stream, ServerState instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			instance.CurrentLoad = 1f;
			instance.GameCount = 0u;
			instance.PlayerCount = 0u;
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num != -1)
				{
					int num2 = num;
					switch (num2)
					{
					case 13:
						instance.CurrentLoad = binaryReader.ReadSingle();
						continue;
					case 14:
					case 15:
					{
						IL_8C:
						if (num2 == 24)
						{
							instance.PlayerCount = ProtocolParser.ReadUInt32(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						uint field = key.Field;
						if (field != 0u)
						{
							ProtocolParser.SkipKey(stream, key);
							continue;
						}
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					case 16:
						instance.GameCount = ProtocolParser.ReadUInt32(stream);
						continue;
					}
					goto IL_8C;
				}
				if (limit >= 0L)
				{
					throw new EndOfStreamException();
				}
				return instance;
			}
			if (stream.get_Position() == limit)
			{
				return instance;
			}
			throw new ProtocolBufferException("Read past max limit");
		}

		public void Serialize(Stream stream)
		{
			ServerState.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ServerState instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.HasCurrentLoad)
			{
				stream.WriteByte(13);
				binaryWriter.Write(instance.CurrentLoad);
			}
			if (instance.HasGameCount)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, instance.GameCount);
			}
			if (instance.HasPlayerCount)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt32(stream, instance.PlayerCount);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasCurrentLoad)
			{
				num += 1u;
				num += 4u;
			}
			if (this.HasGameCount)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.GameCount);
			}
			if (this.HasPlayerCount)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.PlayerCount);
			}
			return num;
		}

		public void SetCurrentLoad(float val)
		{
			this.CurrentLoad = val;
		}

		public void SetGameCount(uint val)
		{
			this.GameCount = val;
		}

		public void SetPlayerCount(uint val)
		{
			this.PlayerCount = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasCurrentLoad)
			{
				num ^= this.CurrentLoad.GetHashCode();
			}
			if (this.HasGameCount)
			{
				num ^= this.GameCount.GetHashCode();
			}
			if (this.HasPlayerCount)
			{
				num ^= this.PlayerCount.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ServerState serverState = obj as ServerState;
			return serverState != null && this.HasCurrentLoad == serverState.HasCurrentLoad && (!this.HasCurrentLoad || this.CurrentLoad.Equals(serverState.CurrentLoad)) && this.HasGameCount == serverState.HasGameCount && (!this.HasGameCount || this.GameCount.Equals(serverState.GameCount)) && this.HasPlayerCount == serverState.HasPlayerCount && (!this.HasPlayerCount || this.PlayerCount.Equals(serverState.PlayerCount));
		}

		public static ServerState ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ServerState>(bs, 0, -1);
		}
	}
}
