using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class GameFoundNotification : IProtoBuf
	{
		public bool HasErrorCode;

		private uint _ErrorCode;

		public bool HasGameHandle;

		private GameHandle _GameHandle;

		private List<ConnectInfo> _ConnectInfo = new List<ConnectInfo>();

		public ulong RequestId
		{
			get;
			set;
		}

		public uint ErrorCode
		{
			get
			{
				return this._ErrorCode;
			}
			set
			{
				this._ErrorCode = value;
				this.HasErrorCode = true;
			}
		}

		public GameHandle GameHandle
		{
			get
			{
				return this._GameHandle;
			}
			set
			{
				this._GameHandle = value;
				this.HasGameHandle = (value != null);
			}
		}

		public List<ConnectInfo> ConnectInfo
		{
			get
			{
				return this._ConnectInfo;
			}
			set
			{
				this._ConnectInfo = value;
			}
		}

		public List<ConnectInfo> ConnectInfoList
		{
			get
			{
				return this._ConnectInfo;
			}
		}

		public int ConnectInfoCount
		{
			get
			{
				return this._ConnectInfo.get_Count();
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
			GameFoundNotification.Deserialize(stream, this);
		}

		public static GameFoundNotification Deserialize(Stream stream, GameFoundNotification instance)
		{
			return GameFoundNotification.Deserialize(stream, instance, -1L);
		}

		public static GameFoundNotification DeserializeLengthDelimited(Stream stream)
		{
			GameFoundNotification gameFoundNotification = new GameFoundNotification();
			GameFoundNotification.DeserializeLengthDelimited(stream, gameFoundNotification);
			return gameFoundNotification;
		}

		public static GameFoundNotification DeserializeLengthDelimited(Stream stream, GameFoundNotification instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GameFoundNotification.Deserialize(stream, instance, num);
		}

		public static GameFoundNotification Deserialize(Stream stream, GameFoundNotification instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			instance.ErrorCode = 0u;
			if (instance.ConnectInfo == null)
			{
				instance.ConnectInfo = new List<ConnectInfo>();
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
					if (num2 != 9)
					{
						if (num2 != 16)
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
								else
								{
									instance.ConnectInfo.Add(bnet.protocol.game_master.ConnectInfo.DeserializeLengthDelimited(stream));
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
						else
						{
							instance.ErrorCode = ProtocolParser.ReadUInt32(stream);
						}
					}
					else
					{
						instance.RequestId = binaryReader.ReadUInt64();
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
			GameFoundNotification.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GameFoundNotification instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			stream.WriteByte(9);
			binaryWriter.Write(instance.RequestId);
			if (instance.HasErrorCode)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, instance.ErrorCode);
			}
			if (instance.HasGameHandle)
			{
				stream.WriteByte(26);
				ProtocolParser.WriteUInt32(stream, instance.GameHandle.GetSerializedSize());
				GameHandle.Serialize(stream, instance.GameHandle);
			}
			if (instance.ConnectInfo.get_Count() > 0)
			{
				using (List<ConnectInfo>.Enumerator enumerator = instance.ConnectInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConnectInfo current = enumerator.get_Current();
						stream.WriteByte(34);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.game_master.ConnectInfo.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += 8u;
			if (this.HasErrorCode)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.ErrorCode);
			}
			if (this.HasGameHandle)
			{
				num += 1u;
				uint serializedSize = this.GameHandle.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.ConnectInfo.get_Count() > 0)
			{
				using (List<ConnectInfo>.Enumerator enumerator = this.ConnectInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConnectInfo current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			num += 1u;
			return num;
		}

		public void SetRequestId(ulong val)
		{
			this.RequestId = val;
		}

		public void SetErrorCode(uint val)
		{
			this.ErrorCode = val;
		}

		public void SetGameHandle(GameHandle val)
		{
			this.GameHandle = val;
		}

		public void AddConnectInfo(ConnectInfo val)
		{
			this._ConnectInfo.Add(val);
		}

		public void ClearConnectInfo()
		{
			this._ConnectInfo.Clear();
		}

		public void SetConnectInfo(List<ConnectInfo> val)
		{
			this.ConnectInfo = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.RequestId.GetHashCode();
			if (this.HasErrorCode)
			{
				num ^= this.ErrorCode.GetHashCode();
			}
			if (this.HasGameHandle)
			{
				num ^= this.GameHandle.GetHashCode();
			}
			using (List<ConnectInfo>.Enumerator enumerator = this.ConnectInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ConnectInfo current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GameFoundNotification gameFoundNotification = obj as GameFoundNotification;
			if (gameFoundNotification == null)
			{
				return false;
			}
			if (!this.RequestId.Equals(gameFoundNotification.RequestId))
			{
				return false;
			}
			if (this.HasErrorCode != gameFoundNotification.HasErrorCode || (this.HasErrorCode && !this.ErrorCode.Equals(gameFoundNotification.ErrorCode)))
			{
				return false;
			}
			if (this.HasGameHandle != gameFoundNotification.HasGameHandle || (this.HasGameHandle && !this.GameHandle.Equals(gameFoundNotification.GameHandle)))
			{
				return false;
			}
			if (this.ConnectInfo.get_Count() != gameFoundNotification.ConnectInfo.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.ConnectInfo.get_Count(); i++)
			{
				if (!this.ConnectInfo.get_Item(i).Equals(gameFoundNotification.ConnectInfo.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static GameFoundNotification ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GameFoundNotification>(bs, 0, -1);
		}
	}
}
