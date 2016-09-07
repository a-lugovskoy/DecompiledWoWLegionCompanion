using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.account
{
	public class AccountServiceConfig : IProtoBuf
	{
		private List<AccountServiceRegion> _Region = new List<AccountServiceRegion>();

		public List<AccountServiceRegion> Region
		{
			get
			{
				return this._Region;
			}
			set
			{
				this._Region = value;
			}
		}

		public List<AccountServiceRegion> RegionList
		{
			get
			{
				return this._Region;
			}
		}

		public int RegionCount
		{
			get
			{
				return this._Region.get_Count();
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
			AccountServiceConfig.Deserialize(stream, this);
		}

		public static AccountServiceConfig Deserialize(Stream stream, AccountServiceConfig instance)
		{
			return AccountServiceConfig.Deserialize(stream, instance, -1L);
		}

		public static AccountServiceConfig DeserializeLengthDelimited(Stream stream)
		{
			AccountServiceConfig accountServiceConfig = new AccountServiceConfig();
			AccountServiceConfig.DeserializeLengthDelimited(stream, accountServiceConfig);
			return accountServiceConfig;
		}

		public static AccountServiceConfig DeserializeLengthDelimited(Stream stream, AccountServiceConfig instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return AccountServiceConfig.Deserialize(stream, instance, num);
		}

		public static AccountServiceConfig Deserialize(Stream stream, AccountServiceConfig instance, long limit)
		{
			if (instance.Region == null)
			{
				instance.Region = new List<AccountServiceRegion>();
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
						instance.Region.Add(AccountServiceRegion.DeserializeLengthDelimited(stream));
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
			AccountServiceConfig.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, AccountServiceConfig instance)
		{
			if (instance.Region.get_Count() > 0)
			{
				using (List<AccountServiceRegion>.Enumerator enumerator = instance.Region.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AccountServiceRegion current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						AccountServiceRegion.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Region.get_Count() > 0)
			{
				using (List<AccountServiceRegion>.Enumerator enumerator = this.Region.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AccountServiceRegion current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddRegion(AccountServiceRegion val)
		{
			this._Region.Add(val);
		}

		public void ClearRegion()
		{
			this._Region.Clear();
		}

		public void SetRegion(List<AccountServiceRegion> val)
		{
			this.Region = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<AccountServiceRegion>.Enumerator enumerator = this.Region.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AccountServiceRegion current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			AccountServiceConfig accountServiceConfig = obj as AccountServiceConfig;
			if (accountServiceConfig == null)
			{
				return false;
			}
			if (this.Region.get_Count() != accountServiceConfig.Region.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Region.get_Count(); i++)
			{
				if (!this.Region.get_Item(i).Equals(accountServiceConfig.Region.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static AccountServiceConfig ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<AccountServiceConfig>(bs, 0, -1);
		}
	}
}
