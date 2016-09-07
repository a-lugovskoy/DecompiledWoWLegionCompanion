using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.account
{
	public class GetLicensesResponse : IProtoBuf
	{
		private List<AccountLicense> _Licenses = new List<AccountLicense>();

		public List<AccountLicense> Licenses
		{
			get
			{
				return this._Licenses;
			}
			set
			{
				this._Licenses = value;
			}
		}

		public List<AccountLicense> LicensesList
		{
			get
			{
				return this._Licenses;
			}
		}

		public int LicensesCount
		{
			get
			{
				return this._Licenses.get_Count();
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
			GetLicensesResponse.Deserialize(stream, this);
		}

		public static GetLicensesResponse Deserialize(Stream stream, GetLicensesResponse instance)
		{
			return GetLicensesResponse.Deserialize(stream, instance, -1L);
		}

		public static GetLicensesResponse DeserializeLengthDelimited(Stream stream)
		{
			GetLicensesResponse getLicensesResponse = new GetLicensesResponse();
			GetLicensesResponse.DeserializeLengthDelimited(stream, getLicensesResponse);
			return getLicensesResponse;
		}

		public static GetLicensesResponse DeserializeLengthDelimited(Stream stream, GetLicensesResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GetLicensesResponse.Deserialize(stream, instance, num);
		}

		public static GetLicensesResponse Deserialize(Stream stream, GetLicensesResponse instance, long limit)
		{
			if (instance.Licenses == null)
			{
				instance.Licenses = new List<AccountLicense>();
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
						instance.Licenses.Add(AccountLicense.DeserializeLengthDelimited(stream));
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
			GetLicensesResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GetLicensesResponse instance)
		{
			if (instance.Licenses.get_Count() > 0)
			{
				using (List<AccountLicense>.Enumerator enumerator = instance.Licenses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AccountLicense current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						AccountLicense.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Licenses.get_Count() > 0)
			{
				using (List<AccountLicense>.Enumerator enumerator = this.Licenses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AccountLicense current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddLicenses(AccountLicense val)
		{
			this._Licenses.Add(val);
		}

		public void ClearLicenses()
		{
			this._Licenses.Clear();
		}

		public void SetLicenses(List<AccountLicense> val)
		{
			this.Licenses = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<AccountLicense>.Enumerator enumerator = this.Licenses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AccountLicense current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GetLicensesResponse getLicensesResponse = obj as GetLicensesResponse;
			if (getLicensesResponse == null)
			{
				return false;
			}
			if (this.Licenses.get_Count() != getLicensesResponse.Licenses.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Licenses.get_Count(); i++)
			{
				if (!this.Licenses.get_Item(i).Equals(getLicensesResponse.Licenses.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static GetLicensesResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GetLicensesResponse>(bs, 0, -1);
		}
	}
}
