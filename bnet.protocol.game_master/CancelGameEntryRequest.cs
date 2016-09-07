using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class CancelGameEntryRequest : IProtoBuf
	{
		public bool HasFactoryId;

		private ulong _FactoryId;

		private List<Player> _Player = new List<Player>();

		public ulong RequestId
		{
			get;
			set;
		}

		public ulong FactoryId
		{
			get
			{
				return this._FactoryId;
			}
			set
			{
				this._FactoryId = value;
				this.HasFactoryId = true;
			}
		}

		public List<Player> Player
		{
			get
			{
				return this._Player;
			}
			set
			{
				this._Player = value;
			}
		}

		public List<Player> PlayerList
		{
			get
			{
				return this._Player;
			}
		}

		public int PlayerCount
		{
			get
			{
				return this._Player.get_Count();
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
			CancelGameEntryRequest.Deserialize(stream, this);
		}

		public static CancelGameEntryRequest Deserialize(Stream stream, CancelGameEntryRequest instance)
		{
			return CancelGameEntryRequest.Deserialize(stream, instance, -1L);
		}

		public static CancelGameEntryRequest DeserializeLengthDelimited(Stream stream)
		{
			CancelGameEntryRequest cancelGameEntryRequest = new CancelGameEntryRequest();
			CancelGameEntryRequest.DeserializeLengthDelimited(stream, cancelGameEntryRequest);
			return cancelGameEntryRequest;
		}

		public static CancelGameEntryRequest DeserializeLengthDelimited(Stream stream, CancelGameEntryRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return CancelGameEntryRequest.Deserialize(stream, instance, num);
		}

		public static CancelGameEntryRequest Deserialize(Stream stream, CancelGameEntryRequest instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.Player == null)
			{
				instance.Player = new List<Player>();
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
						if (num2 != 17)
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
								instance.Player.Add(bnet.protocol.game_master.Player.DeserializeLengthDelimited(stream));
							}
						}
						else
						{
							instance.FactoryId = binaryReader.ReadUInt64();
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
			CancelGameEntryRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, CancelGameEntryRequest instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			stream.WriteByte(9);
			binaryWriter.Write(instance.RequestId);
			if (instance.HasFactoryId)
			{
				stream.WriteByte(17);
				binaryWriter.Write(instance.FactoryId);
			}
			if (instance.Player.get_Count() > 0)
			{
				using (List<Player>.Enumerator enumerator = instance.Player.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Player current = enumerator.get_Current();
						stream.WriteByte(26);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.game_master.Player.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += 8u;
			if (this.HasFactoryId)
			{
				num += 1u;
				num += 8u;
			}
			if (this.Player.get_Count() > 0)
			{
				using (List<Player>.Enumerator enumerator = this.Player.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Player current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
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

		public void SetFactoryId(ulong val)
		{
			this.FactoryId = val;
		}

		public void AddPlayer(Player val)
		{
			this._Player.Add(val);
		}

		public void ClearPlayer()
		{
			this._Player.Clear();
		}

		public void SetPlayer(List<Player> val)
		{
			this.Player = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.RequestId.GetHashCode();
			if (this.HasFactoryId)
			{
				num ^= this.FactoryId.GetHashCode();
			}
			using (List<Player>.Enumerator enumerator = this.Player.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Player current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			CancelGameEntryRequest cancelGameEntryRequest = obj as CancelGameEntryRequest;
			if (cancelGameEntryRequest == null)
			{
				return false;
			}
			if (!this.RequestId.Equals(cancelGameEntryRequest.RequestId))
			{
				return false;
			}
			if (this.HasFactoryId != cancelGameEntryRequest.HasFactoryId || (this.HasFactoryId && !this.FactoryId.Equals(cancelGameEntryRequest.FactoryId)))
			{
				return false;
			}
			if (this.Player.get_Count() != cancelGameEntryRequest.Player.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Player.get_Count(); i++)
			{
				if (!this.Player.get_Item(i).Equals(cancelGameEntryRequest.Player.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static CancelGameEntryRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<CancelGameEntryRequest>(bs, 0, -1);
		}
	}
}
