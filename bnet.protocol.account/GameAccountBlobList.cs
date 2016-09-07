using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.account
{
	public class GameAccountBlobList : IProtoBuf
	{
		private List<GameAccountBlob> _Blob = new List<GameAccountBlob>();

		public List<GameAccountBlob> Blob
		{
			get
			{
				return this._Blob;
			}
			set
			{
				this._Blob = value;
			}
		}

		public List<GameAccountBlob> BlobList
		{
			get
			{
				return this._Blob;
			}
		}

		public int BlobCount
		{
			get
			{
				return this._Blob.get_Count();
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
			GameAccountBlobList.Deserialize(stream, this);
		}

		public static GameAccountBlobList Deserialize(Stream stream, GameAccountBlobList instance)
		{
			return GameAccountBlobList.Deserialize(stream, instance, -1L);
		}

		public static GameAccountBlobList DeserializeLengthDelimited(Stream stream)
		{
			GameAccountBlobList gameAccountBlobList = new GameAccountBlobList();
			GameAccountBlobList.DeserializeLengthDelimited(stream, gameAccountBlobList);
			return gameAccountBlobList;
		}

		public static GameAccountBlobList DeserializeLengthDelimited(Stream stream, GameAccountBlobList instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GameAccountBlobList.Deserialize(stream, instance, num);
		}

		public static GameAccountBlobList Deserialize(Stream stream, GameAccountBlobList instance, long limit)
		{
			if (instance.Blob == null)
			{
				instance.Blob = new List<GameAccountBlob>();
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
						instance.Blob.Add(GameAccountBlob.DeserializeLengthDelimited(stream));
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
			GameAccountBlobList.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GameAccountBlobList instance)
		{
			if (instance.Blob.get_Count() > 0)
			{
				using (List<GameAccountBlob>.Enumerator enumerator = instance.Blob.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameAccountBlob current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						GameAccountBlob.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Blob.get_Count() > 0)
			{
				using (List<GameAccountBlob>.Enumerator enumerator = this.Blob.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameAccountBlob current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddBlob(GameAccountBlob val)
		{
			this._Blob.Add(val);
		}

		public void ClearBlob()
		{
			this._Blob.Clear();
		}

		public void SetBlob(List<GameAccountBlob> val)
		{
			this.Blob = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<GameAccountBlob>.Enumerator enumerator = this.Blob.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameAccountBlob current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GameAccountBlobList gameAccountBlobList = obj as GameAccountBlobList;
			if (gameAccountBlobList == null)
			{
				return false;
			}
			if (this.Blob.get_Count() != gameAccountBlobList.Blob.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Blob.get_Count(); i++)
			{
				if (!this.Blob.get_Item(i).Equals(gameAccountBlobList.Blob.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static GameAccountBlobList ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GameAccountBlobList>(bs, 0, -1);
		}
	}
}
