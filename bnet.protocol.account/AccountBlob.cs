using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol.account
{
	public class AccountBlob : IProtoBuf
	{
		private List<string> _Email = new List<string>();

		public bool HasSecureRelease;

		private ulong _SecureRelease;

		public bool HasWhitelistStart;

		private ulong _WhitelistStart;

		public bool HasWhitelistEnd;

		private ulong _WhitelistEnd;

		private List<AccountLicense> _Licenses = new List<AccountLicense>();

		private List<AccountCredential> _Credentials = new List<AccountCredential>();

		private List<GameAccountLink> _AccountLinks = new List<GameAccountLink>();

		public bool HasBattleTag;

		private string _BattleTag;

		public bool HasDefaultCurrency;

		private uint _DefaultCurrency;

		public bool HasLegalRegion;

		private uint _LegalRegion;

		public bool HasLegalLocale;

		private uint _LegalLocale;

		public bool HasParentalControlInfo;

		private ParentalControlInfo _ParentalControlInfo;

		public bool HasCountry;

		private string _Country;

		public bool HasPreferredRegion;

		private uint _PreferredRegion;

		public uint Id
		{
			get;
			set;
		}

		public uint Region
		{
			get;
			set;
		}

		public List<string> Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				this._Email = value;
			}
		}

		public List<string> EmailList
		{
			get
			{
				return this._Email;
			}
		}

		public int EmailCount
		{
			get
			{
				return this._Email.get_Count();
			}
		}

		public ulong Flags
		{
			get;
			set;
		}

		public ulong SecureRelease
		{
			get
			{
				return this._SecureRelease;
			}
			set
			{
				this._SecureRelease = value;
				this.HasSecureRelease = true;
			}
		}

		public ulong WhitelistStart
		{
			get
			{
				return this._WhitelistStart;
			}
			set
			{
				this._WhitelistStart = value;
				this.HasWhitelistStart = true;
			}
		}

		public ulong WhitelistEnd
		{
			get
			{
				return this._WhitelistEnd;
			}
			set
			{
				this._WhitelistEnd = value;
				this.HasWhitelistEnd = true;
			}
		}

		public string FullName
		{
			get;
			set;
		}

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

		public List<AccountCredential> Credentials
		{
			get
			{
				return this._Credentials;
			}
			set
			{
				this._Credentials = value;
			}
		}

		public List<AccountCredential> CredentialsList
		{
			get
			{
				return this._Credentials;
			}
		}

		public int CredentialsCount
		{
			get
			{
				return this._Credentials.get_Count();
			}
		}

		public List<GameAccountLink> AccountLinks
		{
			get
			{
				return this._AccountLinks;
			}
			set
			{
				this._AccountLinks = value;
			}
		}

		public List<GameAccountLink> AccountLinksList
		{
			get
			{
				return this._AccountLinks;
			}
		}

		public int AccountLinksCount
		{
			get
			{
				return this._AccountLinks.get_Count();
			}
		}

		public string BattleTag
		{
			get
			{
				return this._BattleTag;
			}
			set
			{
				this._BattleTag = value;
				this.HasBattleTag = (value != null);
			}
		}

		public uint DefaultCurrency
		{
			get
			{
				return this._DefaultCurrency;
			}
			set
			{
				this._DefaultCurrency = value;
				this.HasDefaultCurrency = true;
			}
		}

		public uint LegalRegion
		{
			get
			{
				return this._LegalRegion;
			}
			set
			{
				this._LegalRegion = value;
				this.HasLegalRegion = true;
			}
		}

		public uint LegalLocale
		{
			get
			{
				return this._LegalLocale;
			}
			set
			{
				this._LegalLocale = value;
				this.HasLegalLocale = true;
			}
		}

		public ulong CacheExpiration
		{
			get;
			set;
		}

		public ParentalControlInfo ParentalControlInfo
		{
			get
			{
				return this._ParentalControlInfo;
			}
			set
			{
				this._ParentalControlInfo = value;
				this.HasParentalControlInfo = (value != null);
			}
		}

		public string Country
		{
			get
			{
				return this._Country;
			}
			set
			{
				this._Country = value;
				this.HasCountry = (value != null);
			}
		}

		public uint PreferredRegion
		{
			get
			{
				return this._PreferredRegion;
			}
			set
			{
				this._PreferredRegion = value;
				this.HasPreferredRegion = true;
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
			AccountBlob.Deserialize(stream, this);
		}

		public static AccountBlob Deserialize(Stream stream, AccountBlob instance)
		{
			return AccountBlob.Deserialize(stream, instance, -1L);
		}

		public static AccountBlob DeserializeLengthDelimited(Stream stream)
		{
			AccountBlob accountBlob = new AccountBlob();
			AccountBlob.DeserializeLengthDelimited(stream, accountBlob);
			return accountBlob;
		}

		public static AccountBlob DeserializeLengthDelimited(Stream stream, AccountBlob instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return AccountBlob.Deserialize(stream, instance, num);
		}

		public static AccountBlob Deserialize(Stream stream, AccountBlob instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.Email == null)
			{
				instance.Email = new List<string>();
			}
			if (instance.Licenses == null)
			{
				instance.Licenses = new List<AccountLicense>();
			}
			if (instance.Credentials == null)
			{
				instance.Credentials = new List<AccountCredential>();
			}
			if (instance.AccountLinks == null)
			{
				instance.AccountLinks = new List<GameAccountLink>();
			}
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num != -1)
				{
					int num2 = num;
					switch (num2)
					{
					case 21:
						instance.Id = binaryReader.ReadUInt32();
						continue;
					case 22:
					case 23:
						IL_CB:
						if (num2 == 34)
						{
							instance.Email.Add(ProtocolParser.ReadString(stream));
							continue;
						}
						if (num2 == 40)
						{
							instance.Flags = ProtocolParser.ReadUInt64(stream);
							continue;
						}
						if (num2 == 48)
						{
							instance.SecureRelease = ProtocolParser.ReadUInt64(stream);
							continue;
						}
						if (num2 == 56)
						{
							instance.WhitelistStart = ProtocolParser.ReadUInt64(stream);
							continue;
						}
						if (num2 == 64)
						{
							instance.WhitelistEnd = ProtocolParser.ReadUInt64(stream);
							continue;
						}
						if (num2 != 82)
						{
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							uint field = key.Field;
							switch (field)
							{
							case 20u:
								if (key.WireType != Wire.LengthDelimited)
								{
									continue;
								}
								instance.Licenses.Add(AccountLicense.DeserializeLengthDelimited(stream));
								continue;
							case 21u:
								if (key.WireType != Wire.LengthDelimited)
								{
									continue;
								}
								instance.Credentials.Add(AccountCredential.DeserializeLengthDelimited(stream));
								continue;
							case 22u:
								if (key.WireType != Wire.LengthDelimited)
								{
									continue;
								}
								instance.AccountLinks.Add(GameAccountLink.DeserializeLengthDelimited(stream));
								continue;
							case 23u:
								if (key.WireType != Wire.LengthDelimited)
								{
									continue;
								}
								instance.BattleTag = ProtocolParser.ReadString(stream);
								continue;
							case 24u:
							case 28u:
							case 29u:
								IL_1E0:
								if (field != 0u)
								{
									ProtocolParser.SkipKey(stream, key);
									continue;
								}
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							case 25u:
								if (key.WireType != Wire.Fixed32)
								{
									continue;
								}
								instance.DefaultCurrency = binaryReader.ReadUInt32();
								continue;
							case 26u:
								if (key.WireType != Wire.Varint)
								{
									continue;
								}
								instance.LegalRegion = ProtocolParser.ReadUInt32(stream);
								continue;
							case 27u:
								if (key.WireType != Wire.Fixed32)
								{
									continue;
								}
								instance.LegalLocale = binaryReader.ReadUInt32();
								continue;
							case 30u:
								if (key.WireType != Wire.Varint)
								{
									continue;
								}
								instance.CacheExpiration = ProtocolParser.ReadUInt64(stream);
								continue;
							case 31u:
								if (key.WireType != Wire.LengthDelimited)
								{
									continue;
								}
								if (instance.ParentalControlInfo == null)
								{
									instance.ParentalControlInfo = ParentalControlInfo.DeserializeLengthDelimited(stream);
								}
								else
								{
									ParentalControlInfo.DeserializeLengthDelimited(stream, instance.ParentalControlInfo);
								}
								continue;
							case 32u:
								if (key.WireType != Wire.LengthDelimited)
								{
									continue;
								}
								instance.Country = ProtocolParser.ReadString(stream);
								continue;
							case 33u:
								if (key.WireType != Wire.Varint)
								{
									continue;
								}
								instance.PreferredRegion = ProtocolParser.ReadUInt32(stream);
								continue;
							}
							goto IL_1E0;
							continue;
						}
						instance.FullName = ProtocolParser.ReadString(stream);
						continue;
					case 24:
						instance.Region = ProtocolParser.ReadUInt32(stream);
						continue;
					}
					goto IL_CB;
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
			AccountBlob.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, AccountBlob instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			stream.WriteByte(21);
			binaryWriter.Write(instance.Id);
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.Region);
			if (instance.Email.get_Count() > 0)
			{
				using (List<string>.Enumerator enumerator = instance.Email.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						stream.WriteByte(34);
						ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(current));
					}
				}
			}
			stream.WriteByte(40);
			ProtocolParser.WriteUInt64(stream, instance.Flags);
			if (instance.HasSecureRelease)
			{
				stream.WriteByte(48);
				ProtocolParser.WriteUInt64(stream, instance.SecureRelease);
			}
			if (instance.HasWhitelistStart)
			{
				stream.WriteByte(56);
				ProtocolParser.WriteUInt64(stream, instance.WhitelistStart);
			}
			if (instance.HasWhitelistEnd)
			{
				stream.WriteByte(64);
				ProtocolParser.WriteUInt64(stream, instance.WhitelistEnd);
			}
			if (instance.FullName == null)
			{
				throw new ArgumentNullException("FullName", "Required by proto specification.");
			}
			stream.WriteByte(82);
			ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.FullName));
			if (instance.Licenses.get_Count() > 0)
			{
				using (List<AccountLicense>.Enumerator enumerator2 = instance.Licenses.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AccountLicense current2 = enumerator2.get_Current();
						stream.WriteByte(162);
						stream.WriteByte(1);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						AccountLicense.Serialize(stream, current2);
					}
				}
			}
			if (instance.Credentials.get_Count() > 0)
			{
				using (List<AccountCredential>.Enumerator enumerator3 = instance.Credentials.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						AccountCredential current3 = enumerator3.get_Current();
						stream.WriteByte(170);
						stream.WriteByte(1);
						ProtocolParser.WriteUInt32(stream, current3.GetSerializedSize());
						AccountCredential.Serialize(stream, current3);
					}
				}
			}
			if (instance.AccountLinks.get_Count() > 0)
			{
				using (List<GameAccountLink>.Enumerator enumerator4 = instance.AccountLinks.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						GameAccountLink current4 = enumerator4.get_Current();
						stream.WriteByte(178);
						stream.WriteByte(1);
						ProtocolParser.WriteUInt32(stream, current4.GetSerializedSize());
						GameAccountLink.Serialize(stream, current4);
					}
				}
			}
			if (instance.HasBattleTag)
			{
				stream.WriteByte(186);
				stream.WriteByte(1);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.BattleTag));
			}
			if (instance.HasDefaultCurrency)
			{
				stream.WriteByte(205);
				stream.WriteByte(1);
				binaryWriter.Write(instance.DefaultCurrency);
			}
			if (instance.HasLegalRegion)
			{
				stream.WriteByte(208);
				stream.WriteByte(1);
				ProtocolParser.WriteUInt32(stream, instance.LegalRegion);
			}
			if (instance.HasLegalLocale)
			{
				stream.WriteByte(221);
				stream.WriteByte(1);
				binaryWriter.Write(instance.LegalLocale);
			}
			stream.WriteByte(240);
			stream.WriteByte(1);
			ProtocolParser.WriteUInt64(stream, instance.CacheExpiration);
			if (instance.HasParentalControlInfo)
			{
				stream.WriteByte(250);
				stream.WriteByte(1);
				ProtocolParser.WriteUInt32(stream, instance.ParentalControlInfo.GetSerializedSize());
				ParentalControlInfo.Serialize(stream, instance.ParentalControlInfo);
			}
			if (instance.HasCountry)
			{
				stream.WriteByte(130);
				stream.WriteByte(2);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Country));
			}
			if (instance.HasPreferredRegion)
			{
				stream.WriteByte(136);
				stream.WriteByte(2);
				ProtocolParser.WriteUInt32(stream, instance.PreferredRegion);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += 4u;
			num += ProtocolParser.SizeOfUInt32(this.Region);
			if (this.Email.get_Count() > 0)
			{
				using (List<string>.Enumerator enumerator = this.Email.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						num += 1u;
						uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(current);
						num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
					}
				}
			}
			num += ProtocolParser.SizeOfUInt64(this.Flags);
			if (this.HasSecureRelease)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64(this.SecureRelease);
			}
			if (this.HasWhitelistStart)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64(this.WhitelistStart);
			}
			if (this.HasWhitelistEnd)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64(this.WhitelistEnd);
			}
			uint byteCount2 = (uint)Encoding.get_UTF8().GetByteCount(this.FullName);
			num += ProtocolParser.SizeOfUInt32(byteCount2) + byteCount2;
			if (this.Licenses.get_Count() > 0)
			{
				using (List<AccountLicense>.Enumerator enumerator2 = this.Licenses.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AccountLicense current2 = enumerator2.get_Current();
						num += 2u;
						uint serializedSize = current2.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.Credentials.get_Count() > 0)
			{
				using (List<AccountCredential>.Enumerator enumerator3 = this.Credentials.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						AccountCredential current3 = enumerator3.get_Current();
						num += 2u;
						uint serializedSize2 = current3.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			if (this.AccountLinks.get_Count() > 0)
			{
				using (List<GameAccountLink>.Enumerator enumerator4 = this.AccountLinks.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						GameAccountLink current4 = enumerator4.get_Current();
						num += 2u;
						uint serializedSize3 = current4.GetSerializedSize();
						num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
					}
				}
			}
			if (this.HasBattleTag)
			{
				num += 2u;
				uint byteCount3 = (uint)Encoding.get_UTF8().GetByteCount(this.BattleTag);
				num += ProtocolParser.SizeOfUInt32(byteCount3) + byteCount3;
			}
			if (this.HasDefaultCurrency)
			{
				num += 2u;
				num += 4u;
			}
			if (this.HasLegalRegion)
			{
				num += 2u;
				num += ProtocolParser.SizeOfUInt32(this.LegalRegion);
			}
			if (this.HasLegalLocale)
			{
				num += 2u;
				num += 4u;
			}
			num += ProtocolParser.SizeOfUInt64(this.CacheExpiration);
			if (this.HasParentalControlInfo)
			{
				num += 2u;
				uint serializedSize4 = this.ParentalControlInfo.GetSerializedSize();
				num += serializedSize4 + ProtocolParser.SizeOfUInt32(serializedSize4);
			}
			if (this.HasCountry)
			{
				num += 2u;
				uint byteCount4 = (uint)Encoding.get_UTF8().GetByteCount(this.Country);
				num += ProtocolParser.SizeOfUInt32(byteCount4) + byteCount4;
			}
			if (this.HasPreferredRegion)
			{
				num += 2u;
				num += ProtocolParser.SizeOfUInt32(this.PreferredRegion);
			}
			num += 6u;
			return num;
		}

		public void SetId(uint val)
		{
			this.Id = val;
		}

		public void SetRegion(uint val)
		{
			this.Region = val;
		}

		public void AddEmail(string val)
		{
			this._Email.Add(val);
		}

		public void ClearEmail()
		{
			this._Email.Clear();
		}

		public void SetEmail(List<string> val)
		{
			this.Email = val;
		}

		public void SetFlags(ulong val)
		{
			this.Flags = val;
		}

		public void SetSecureRelease(ulong val)
		{
			this.SecureRelease = val;
		}

		public void SetWhitelistStart(ulong val)
		{
			this.WhitelistStart = val;
		}

		public void SetWhitelistEnd(ulong val)
		{
			this.WhitelistEnd = val;
		}

		public void SetFullName(string val)
		{
			this.FullName = val;
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

		public void AddCredentials(AccountCredential val)
		{
			this._Credentials.Add(val);
		}

		public void ClearCredentials()
		{
			this._Credentials.Clear();
		}

		public void SetCredentials(List<AccountCredential> val)
		{
			this.Credentials = val;
		}

		public void AddAccountLinks(GameAccountLink val)
		{
			this._AccountLinks.Add(val);
		}

		public void ClearAccountLinks()
		{
			this._AccountLinks.Clear();
		}

		public void SetAccountLinks(List<GameAccountLink> val)
		{
			this.AccountLinks = val;
		}

		public void SetBattleTag(string val)
		{
			this.BattleTag = val;
		}

		public void SetDefaultCurrency(uint val)
		{
			this.DefaultCurrency = val;
		}

		public void SetLegalRegion(uint val)
		{
			this.LegalRegion = val;
		}

		public void SetLegalLocale(uint val)
		{
			this.LegalLocale = val;
		}

		public void SetCacheExpiration(ulong val)
		{
			this.CacheExpiration = val;
		}

		public void SetParentalControlInfo(ParentalControlInfo val)
		{
			this.ParentalControlInfo = val;
		}

		public void SetCountry(string val)
		{
			this.Country = val;
		}

		public void SetPreferredRegion(uint val)
		{
			this.PreferredRegion = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.Id.GetHashCode();
			num ^= this.Region.GetHashCode();
			using (List<string>.Enumerator enumerator = this.Email.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			num ^= this.Flags.GetHashCode();
			if (this.HasSecureRelease)
			{
				num ^= this.SecureRelease.GetHashCode();
			}
			if (this.HasWhitelistStart)
			{
				num ^= this.WhitelistStart.GetHashCode();
			}
			if (this.HasWhitelistEnd)
			{
				num ^= this.WhitelistEnd.GetHashCode();
			}
			num ^= this.FullName.GetHashCode();
			using (List<AccountLicense>.Enumerator enumerator2 = this.Licenses.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					AccountLicense current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			using (List<AccountCredential>.Enumerator enumerator3 = this.Credentials.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					AccountCredential current3 = enumerator3.get_Current();
					num ^= current3.GetHashCode();
				}
			}
			using (List<GameAccountLink>.Enumerator enumerator4 = this.AccountLinks.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					GameAccountLink current4 = enumerator4.get_Current();
					num ^= current4.GetHashCode();
				}
			}
			if (this.HasBattleTag)
			{
				num ^= this.BattleTag.GetHashCode();
			}
			if (this.HasDefaultCurrency)
			{
				num ^= this.DefaultCurrency.GetHashCode();
			}
			if (this.HasLegalRegion)
			{
				num ^= this.LegalRegion.GetHashCode();
			}
			if (this.HasLegalLocale)
			{
				num ^= this.LegalLocale.GetHashCode();
			}
			num ^= this.CacheExpiration.GetHashCode();
			if (this.HasParentalControlInfo)
			{
				num ^= this.ParentalControlInfo.GetHashCode();
			}
			if (this.HasCountry)
			{
				num ^= this.Country.GetHashCode();
			}
			if (this.HasPreferredRegion)
			{
				num ^= this.PreferredRegion.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			AccountBlob accountBlob = obj as AccountBlob;
			if (accountBlob == null)
			{
				return false;
			}
			if (!this.Id.Equals(accountBlob.Id))
			{
				return false;
			}
			if (!this.Region.Equals(accountBlob.Region))
			{
				return false;
			}
			if (this.Email.get_Count() != accountBlob.Email.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Email.get_Count(); i++)
			{
				if (!this.Email.get_Item(i).Equals(accountBlob.Email.get_Item(i)))
				{
					return false;
				}
			}
			if (!this.Flags.Equals(accountBlob.Flags))
			{
				return false;
			}
			if (this.HasSecureRelease != accountBlob.HasSecureRelease || (this.HasSecureRelease && !this.SecureRelease.Equals(accountBlob.SecureRelease)))
			{
				return false;
			}
			if (this.HasWhitelistStart != accountBlob.HasWhitelistStart || (this.HasWhitelistStart && !this.WhitelistStart.Equals(accountBlob.WhitelistStart)))
			{
				return false;
			}
			if (this.HasWhitelistEnd != accountBlob.HasWhitelistEnd || (this.HasWhitelistEnd && !this.WhitelistEnd.Equals(accountBlob.WhitelistEnd)))
			{
				return false;
			}
			if (!this.FullName.Equals(accountBlob.FullName))
			{
				return false;
			}
			if (this.Licenses.get_Count() != accountBlob.Licenses.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.Licenses.get_Count(); j++)
			{
				if (!this.Licenses.get_Item(j).Equals(accountBlob.Licenses.get_Item(j)))
				{
					return false;
				}
			}
			if (this.Credentials.get_Count() != accountBlob.Credentials.get_Count())
			{
				return false;
			}
			for (int k = 0; k < this.Credentials.get_Count(); k++)
			{
				if (!this.Credentials.get_Item(k).Equals(accountBlob.Credentials.get_Item(k)))
				{
					return false;
				}
			}
			if (this.AccountLinks.get_Count() != accountBlob.AccountLinks.get_Count())
			{
				return false;
			}
			for (int l = 0; l < this.AccountLinks.get_Count(); l++)
			{
				if (!this.AccountLinks.get_Item(l).Equals(accountBlob.AccountLinks.get_Item(l)))
				{
					return false;
				}
			}
			return this.HasBattleTag == accountBlob.HasBattleTag && (!this.HasBattleTag || this.BattleTag.Equals(accountBlob.BattleTag)) && this.HasDefaultCurrency == accountBlob.HasDefaultCurrency && (!this.HasDefaultCurrency || this.DefaultCurrency.Equals(accountBlob.DefaultCurrency)) && this.HasLegalRegion == accountBlob.HasLegalRegion && (!this.HasLegalRegion || this.LegalRegion.Equals(accountBlob.LegalRegion)) && this.HasLegalLocale == accountBlob.HasLegalLocale && (!this.HasLegalLocale || this.LegalLocale.Equals(accountBlob.LegalLocale)) && this.CacheExpiration.Equals(accountBlob.CacheExpiration) && this.HasParentalControlInfo == accountBlob.HasParentalControlInfo && (!this.HasParentalControlInfo || this.ParentalControlInfo.Equals(accountBlob.ParentalControlInfo)) && this.HasCountry == accountBlob.HasCountry && (!this.HasCountry || this.Country.Equals(accountBlob.Country)) && this.HasPreferredRegion == accountBlob.HasPreferredRegion && (!this.HasPreferredRegion || this.PreferredRegion.Equals(accountBlob.PreferredRegion));
		}

		public static AccountBlob ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<AccountBlob>(bs, 0, -1);
		}
	}
}
