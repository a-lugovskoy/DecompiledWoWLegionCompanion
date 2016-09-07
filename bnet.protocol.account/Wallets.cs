using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.account
{
	public class Wallets : IProtoBuf
	{
		private List<Wallet> _Wallets_ = new List<Wallet>();

		public List<Wallet> Wallets_
		{
			get
			{
				return this._Wallets_;
			}
			set
			{
				this._Wallets_ = value;
			}
		}

		public List<Wallet> Wallets_List
		{
			get
			{
				return this._Wallets_;
			}
		}

		public int Wallets_Count
		{
			get
			{
				return this._Wallets_.get_Count();
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
			Wallets.Deserialize(stream, this);
		}

		public static Wallets Deserialize(Stream stream, Wallets instance)
		{
			return Wallets.Deserialize(stream, instance, -1L);
		}

		public static Wallets DeserializeLengthDelimited(Stream stream)
		{
			Wallets wallets = new Wallets();
			Wallets.DeserializeLengthDelimited(stream, wallets);
			return wallets;
		}

		public static Wallets DeserializeLengthDelimited(Stream stream, Wallets instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return Wallets.Deserialize(stream, instance, num);
		}

		public static Wallets Deserialize(Stream stream, Wallets instance, long limit)
		{
			if (instance.Wallets_ == null)
			{
				instance.Wallets_ = new List<Wallet>();
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
						instance.Wallets_.Add(Wallet.DeserializeLengthDelimited(stream));
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
			Wallets.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, Wallets instance)
		{
			if (instance.Wallets_.get_Count() > 0)
			{
				using (List<Wallet>.Enumerator enumerator = instance.Wallets_.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Wallet current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						Wallet.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Wallets_.get_Count() > 0)
			{
				using (List<Wallet>.Enumerator enumerator = this.Wallets_.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Wallet current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddWallets_(Wallet val)
		{
			this._Wallets_.Add(val);
		}

		public void ClearWallets_()
		{
			this._Wallets_.Clear();
		}

		public void SetWallets_(List<Wallet> val)
		{
			this.Wallets_ = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<Wallet>.Enumerator enumerator = this.Wallets_.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Wallet current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			Wallets wallets = obj as Wallets;
			if (wallets == null)
			{
				return false;
			}
			if (this.Wallets_.get_Count() != wallets.Wallets_.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Wallets_.get_Count(); i++)
			{
				if (!this.Wallets_.get_Item(i).Equals(wallets.Wallets_.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static Wallets ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<Wallets>(bs, 0, -1);
		}
	}
}
