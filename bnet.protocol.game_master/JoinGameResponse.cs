using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class JoinGameResponse : IProtoBuf
	{
		public bool HasRequestId;

		private ulong _RequestId;

		public bool HasQueued;

		private bool _Queued;

		private List<ConnectInfo> _ConnectInfo = new List<ConnectInfo>();

		public ulong RequestId
		{
			get
			{
				return this._RequestId;
			}
			set
			{
				this._RequestId = value;
				this.HasRequestId = true;
			}
		}

		public bool Queued
		{
			get
			{
				return this._Queued;
			}
			set
			{
				this._Queued = value;
				this.HasQueued = true;
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
			JoinGameResponse.Deserialize(stream, this);
		}

		public static JoinGameResponse Deserialize(Stream stream, JoinGameResponse instance)
		{
			return JoinGameResponse.Deserialize(stream, instance, -1L);
		}

		public static JoinGameResponse DeserializeLengthDelimited(Stream stream)
		{
			JoinGameResponse joinGameResponse = new JoinGameResponse();
			JoinGameResponse.DeserializeLengthDelimited(stream, joinGameResponse);
			return joinGameResponse;
		}

		public static JoinGameResponse DeserializeLengthDelimited(Stream stream, JoinGameResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return JoinGameResponse.Deserialize(stream, instance, num);
		}

		public static JoinGameResponse Deserialize(Stream stream, JoinGameResponse instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			instance.Queued = false;
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
						else
						{
							instance.Queued = ProtocolParser.ReadBool(stream);
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
			JoinGameResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, JoinGameResponse instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.HasRequestId)
			{
				stream.WriteByte(9);
				binaryWriter.Write(instance.RequestId);
			}
			if (instance.HasQueued)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteBool(stream, instance.Queued);
			}
			if (instance.ConnectInfo.get_Count() > 0)
			{
				using (List<ConnectInfo>.Enumerator enumerator = instance.ConnectInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConnectInfo current = enumerator.get_Current();
						stream.WriteByte(26);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.game_master.ConnectInfo.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasRequestId)
			{
				num += 1u;
				num += 8u;
			}
			if (this.HasQueued)
			{
				num += 1u;
				num += 1u;
			}
			if (this.ConnectInfo.get_Count() > 0)
			{
				using (List<ConnectInfo>.Enumerator enumerator = this.ConnectInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConnectInfo current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void SetRequestId(ulong val)
		{
			this.RequestId = val;
		}

		public void SetQueued(bool val)
		{
			this.Queued = val;
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
			if (this.HasRequestId)
			{
				num ^= this.RequestId.GetHashCode();
			}
			if (this.HasQueued)
			{
				num ^= this.Queued.GetHashCode();
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
			JoinGameResponse joinGameResponse = obj as JoinGameResponse;
			if (joinGameResponse == null)
			{
				return false;
			}
			if (this.HasRequestId != joinGameResponse.HasRequestId || (this.HasRequestId && !this.RequestId.Equals(joinGameResponse.RequestId)))
			{
				return false;
			}
			if (this.HasQueued != joinGameResponse.HasQueued || (this.HasQueued && !this.Queued.Equals(joinGameResponse.Queued)))
			{
				return false;
			}
			if (this.ConnectInfo.get_Count() != joinGameResponse.ConnectInfo.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.ConnectInfo.get_Count(); i++)
			{
				if (!this.ConnectInfo.get_Item(i).Equals(joinGameResponse.ConnectInfo.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static JoinGameResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<JoinGameResponse>(bs, 0, -1);
		}
	}
}
