using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol.account
{
	public class GetAccountResponse : IProtoBuf
	{
		public bool HasBlob;

		private AccountBlob _Blob;

		public bool HasId;

		private AccountId _Id;

		private List<string> _Email = new List<string>();

		public bool HasBattleTag;

		private string _BattleTag;

		public bool HasFullName;

		private string _FullName;

		private List<GameAccountLink> _Links = new List<GameAccountLink>();

		public bool HasParentalControlInfo;

		private ParentalControlInfo _ParentalControlInfo;

		public AccountBlob Blob
		{
			get
			{
				return this._Blob;
			}
			set
			{
				this._Blob = value;
				this.HasBlob = (value != null);
			}
		}

		public AccountId Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				this._Id = value;
				this.HasId = (value != null);
			}
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

		public string FullName
		{
			get
			{
				return this._FullName;
			}
			set
			{
				this._FullName = value;
				this.HasFullName = (value != null);
			}
		}

		public List<GameAccountLink> Links
		{
			get
			{
				return this._Links;
			}
			set
			{
				this._Links = value;
			}
		}

		public List<GameAccountLink> LinksList
		{
			get
			{
				return this._Links;
			}
		}

		public int LinksCount
		{
			get
			{
				return this._Links.get_Count();
			}
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

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			GetAccountResponse.Deserialize(stream, this);
		}

		public static GetAccountResponse Deserialize(Stream stream, GetAccountResponse instance)
		{
			return GetAccountResponse.Deserialize(stream, instance, -1L);
		}

		public static GetAccountResponse DeserializeLengthDelimited(Stream stream)
		{
			GetAccountResponse getAccountResponse = new GetAccountResponse();
			GetAccountResponse.DeserializeLengthDelimited(stream, getAccountResponse);
			return getAccountResponse;
		}

		public static GetAccountResponse DeserializeLengthDelimited(Stream stream, GetAccountResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GetAccountResponse.Deserialize(stream, instance, num);
		}

		public static GetAccountResponse Deserialize(Stream stream, GetAccountResponse instance, long limit)
		{
			if (instance.Email == null)
			{
				instance.Email = new List<string>();
			}
			if (instance.Links == null)
			{
				instance.Links = new List<GameAccountLink>();
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
					if (num2 != 90)
					{
						if (num2 != 98)
						{
							if (num2 != 106)
							{
								if (num2 != 114)
								{
									if (num2 != 122)
									{
										Key key = ProtocolParser.ReadKey((byte)num, stream);
										uint field = key.Field;
										if (field != 16u)
										{
											if (field != 17u)
											{
												if (field == 0u)
												{
													throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
												}
												ProtocolParser.SkipKey(stream, key);
											}
											else if (key.WireType == Wire.LengthDelimited)
											{
												if (instance.ParentalControlInfo == null)
												{
													instance.ParentalControlInfo = ParentalControlInfo.DeserializeLengthDelimited(stream);
												}
												else
												{
													ParentalControlInfo.DeserializeLengthDelimited(stream, instance.ParentalControlInfo);
												}
											}
										}
										else if (key.WireType == Wire.LengthDelimited)
										{
											instance.Links.Add(GameAccountLink.DeserializeLengthDelimited(stream));
										}
									}
									else
									{
										instance.FullName = ProtocolParser.ReadString(stream);
									}
								}
								else
								{
									instance.BattleTag = ProtocolParser.ReadString(stream);
								}
							}
							else
							{
								instance.Email.Add(ProtocolParser.ReadString(stream));
							}
						}
						else if (instance.Id == null)
						{
							instance.Id = AccountId.DeserializeLengthDelimited(stream);
						}
						else
						{
							AccountId.DeserializeLengthDelimited(stream, instance.Id);
						}
					}
					else if (instance.Blob == null)
					{
						instance.Blob = AccountBlob.DeserializeLengthDelimited(stream);
					}
					else
					{
						AccountBlob.DeserializeLengthDelimited(stream, instance.Blob);
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
			GetAccountResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GetAccountResponse instance)
		{
			if (instance.HasBlob)
			{
				stream.WriteByte(90);
				ProtocolParser.WriteUInt32(stream, instance.Blob.GetSerializedSize());
				AccountBlob.Serialize(stream, instance.Blob);
			}
			if (instance.HasId)
			{
				stream.WriteByte(98);
				ProtocolParser.WriteUInt32(stream, instance.Id.GetSerializedSize());
				AccountId.Serialize(stream, instance.Id);
			}
			if (instance.Email.get_Count() > 0)
			{
				using (List<string>.Enumerator enumerator = instance.Email.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						stream.WriteByte(106);
						ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(current));
					}
				}
			}
			if (instance.HasBattleTag)
			{
				stream.WriteByte(114);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.BattleTag));
			}
			if (instance.HasFullName)
			{
				stream.WriteByte(122);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.FullName));
			}
			if (instance.Links.get_Count() > 0)
			{
				using (List<GameAccountLink>.Enumerator enumerator2 = instance.Links.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameAccountLink current2 = enumerator2.get_Current();
						stream.WriteByte(130);
						stream.WriteByte(1);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						GameAccountLink.Serialize(stream, current2);
					}
				}
			}
			if (instance.HasParentalControlInfo)
			{
				stream.WriteByte(138);
				stream.WriteByte(1);
				ProtocolParser.WriteUInt32(stream, instance.ParentalControlInfo.GetSerializedSize());
				ParentalControlInfo.Serialize(stream, instance.ParentalControlInfo);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasBlob)
			{
				num += 1u;
				uint serializedSize = this.Blob.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.HasId)
			{
				num += 1u;
				uint serializedSize2 = this.Id.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
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
			if (this.HasBattleTag)
			{
				num += 1u;
				uint byteCount2 = (uint)Encoding.get_UTF8().GetByteCount(this.BattleTag);
				num += ProtocolParser.SizeOfUInt32(byteCount2) + byteCount2;
			}
			if (this.HasFullName)
			{
				num += 1u;
				uint byteCount3 = (uint)Encoding.get_UTF8().GetByteCount(this.FullName);
				num += ProtocolParser.SizeOfUInt32(byteCount3) + byteCount3;
			}
			if (this.Links.get_Count() > 0)
			{
				using (List<GameAccountLink>.Enumerator enumerator2 = this.Links.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameAccountLink current2 = enumerator2.get_Current();
						num += 2u;
						uint serializedSize3 = current2.GetSerializedSize();
						num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
					}
				}
			}
			if (this.HasParentalControlInfo)
			{
				num += 2u;
				uint serializedSize4 = this.ParentalControlInfo.GetSerializedSize();
				num += serializedSize4 + ProtocolParser.SizeOfUInt32(serializedSize4);
			}
			return num;
		}

		public void SetBlob(AccountBlob val)
		{
			this.Blob = val;
		}

		public void SetId(AccountId val)
		{
			this.Id = val;
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

		public void SetBattleTag(string val)
		{
			this.BattleTag = val;
		}

		public void SetFullName(string val)
		{
			this.FullName = val;
		}

		public void AddLinks(GameAccountLink val)
		{
			this._Links.Add(val);
		}

		public void ClearLinks()
		{
			this._Links.Clear();
		}

		public void SetLinks(List<GameAccountLink> val)
		{
			this.Links = val;
		}

		public void SetParentalControlInfo(ParentalControlInfo val)
		{
			this.ParentalControlInfo = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasBlob)
			{
				num ^= this.Blob.GetHashCode();
			}
			if (this.HasId)
			{
				num ^= this.Id.GetHashCode();
			}
			using (List<string>.Enumerator enumerator = this.Email.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			if (this.HasBattleTag)
			{
				num ^= this.BattleTag.GetHashCode();
			}
			if (this.HasFullName)
			{
				num ^= this.FullName.GetHashCode();
			}
			using (List<GameAccountLink>.Enumerator enumerator2 = this.Links.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameAccountLink current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			if (this.HasParentalControlInfo)
			{
				num ^= this.ParentalControlInfo.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GetAccountResponse getAccountResponse = obj as GetAccountResponse;
			if (getAccountResponse == null)
			{
				return false;
			}
			if (this.HasBlob != getAccountResponse.HasBlob || (this.HasBlob && !this.Blob.Equals(getAccountResponse.Blob)))
			{
				return false;
			}
			if (this.HasId != getAccountResponse.HasId || (this.HasId && !this.Id.Equals(getAccountResponse.Id)))
			{
				return false;
			}
			if (this.Email.get_Count() != getAccountResponse.Email.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Email.get_Count(); i++)
			{
				if (!this.Email.get_Item(i).Equals(getAccountResponse.Email.get_Item(i)))
				{
					return false;
				}
			}
			if (this.HasBattleTag != getAccountResponse.HasBattleTag || (this.HasBattleTag && !this.BattleTag.Equals(getAccountResponse.BattleTag)))
			{
				return false;
			}
			if (this.HasFullName != getAccountResponse.HasFullName || (this.HasFullName && !this.FullName.Equals(getAccountResponse.FullName)))
			{
				return false;
			}
			if (this.Links.get_Count() != getAccountResponse.Links.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.Links.get_Count(); j++)
			{
				if (!this.Links.get_Item(j).Equals(getAccountResponse.Links.get_Item(j)))
				{
					return false;
				}
			}
			return this.HasParentalControlInfo == getAccountResponse.HasParentalControlInfo && (!this.HasParentalControlInfo || this.ParentalControlInfo.Equals(getAccountResponse.ParentalControlInfo));
		}

		public static GetAccountResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GetAccountResponse>(bs, 0, -1);
		}
	}
}
