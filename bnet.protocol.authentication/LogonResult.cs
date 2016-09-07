using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol.authentication
{
	public class LogonResult : IProtoBuf
	{
		public bool HasAccount;

		private EntityId _Account;

		private List<EntityId> _GameAccount = new List<EntityId>();

		public bool HasEmail;

		private string _Email;

		private List<uint> _AvailableRegion = new List<uint>();

		public bool HasConnectedRegion;

		private uint _ConnectedRegion;

		public bool HasBattleTag;

		private string _BattleTag;

		public bool HasGeoipCountry;

		private string _GeoipCountry;

		public bool HasSessionKey;

		private byte[] _SessionKey;

		public uint ErrorCode
		{
			get;
			set;
		}

		public EntityId Account
		{
			get
			{
				return this._Account;
			}
			set
			{
				this._Account = value;
				this.HasAccount = (value != null);
			}
		}

		public List<EntityId> GameAccount
		{
			get
			{
				return this._GameAccount;
			}
			set
			{
				this._GameAccount = value;
			}
		}

		public List<EntityId> GameAccountList
		{
			get
			{
				return this._GameAccount;
			}
		}

		public int GameAccountCount
		{
			get
			{
				return this._GameAccount.get_Count();
			}
		}

		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				this._Email = value;
				this.HasEmail = (value != null);
			}
		}

		public List<uint> AvailableRegion
		{
			get
			{
				return this._AvailableRegion;
			}
			set
			{
				this._AvailableRegion = value;
			}
		}

		public List<uint> AvailableRegionList
		{
			get
			{
				return this._AvailableRegion;
			}
		}

		public int AvailableRegionCount
		{
			get
			{
				return this._AvailableRegion.get_Count();
			}
		}

		public uint ConnectedRegion
		{
			get
			{
				return this._ConnectedRegion;
			}
			set
			{
				this._ConnectedRegion = value;
				this.HasConnectedRegion = true;
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

		public string GeoipCountry
		{
			get
			{
				return this._GeoipCountry;
			}
			set
			{
				this._GeoipCountry = value;
				this.HasGeoipCountry = (value != null);
			}
		}

		public byte[] SessionKey
		{
			get
			{
				return this._SessionKey;
			}
			set
			{
				this._SessionKey = value;
				this.HasSessionKey = (value != null);
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
			LogonResult.Deserialize(stream, this);
		}

		public static LogonResult Deserialize(Stream stream, LogonResult instance)
		{
			return LogonResult.Deserialize(stream, instance, -1L);
		}

		public static LogonResult DeserializeLengthDelimited(Stream stream)
		{
			LogonResult logonResult = new LogonResult();
			LogonResult.DeserializeLengthDelimited(stream, logonResult);
			return logonResult;
		}

		public static LogonResult DeserializeLengthDelimited(Stream stream, LogonResult instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return LogonResult.Deserialize(stream, instance, num);
		}

		public static LogonResult Deserialize(Stream stream, LogonResult instance, long limit)
		{
			if (instance.GameAccount == null)
			{
				instance.GameAccount = new List<EntityId>();
			}
			if (instance.AvailableRegion == null)
			{
				instance.AvailableRegion = new List<uint>();
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
					if (num2 != 8)
					{
						if (num2 != 18)
						{
							if (num2 != 26)
							{
								if (num2 != 34)
								{
									if (num2 != 40)
									{
										if (num2 != 48)
										{
											if (num2 != 58)
											{
												if (num2 != 66)
												{
													if (num2 != 74)
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
														instance.SessionKey = ProtocolParser.ReadBytes(stream);
													}
												}
												else
												{
													instance.GeoipCountry = ProtocolParser.ReadString(stream);
												}
											}
											else
											{
												instance.BattleTag = ProtocolParser.ReadString(stream);
											}
										}
										else
										{
											instance.ConnectedRegion = ProtocolParser.ReadUInt32(stream);
										}
									}
									else
									{
										instance.AvailableRegion.Add(ProtocolParser.ReadUInt32(stream));
									}
								}
								else
								{
									instance.Email = ProtocolParser.ReadString(stream);
								}
							}
							else
							{
								instance.GameAccount.Add(EntityId.DeserializeLengthDelimited(stream));
							}
						}
						else if (instance.Account == null)
						{
							instance.Account = EntityId.DeserializeLengthDelimited(stream);
						}
						else
						{
							EntityId.DeserializeLengthDelimited(stream, instance.Account);
						}
					}
					else
					{
						instance.ErrorCode = ProtocolParser.ReadUInt32(stream);
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
			LogonResult.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, LogonResult instance)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.ErrorCode);
			if (instance.HasAccount)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteUInt32(stream, instance.Account.GetSerializedSize());
				EntityId.Serialize(stream, instance.Account);
			}
			if (instance.GameAccount.get_Count() > 0)
			{
				using (List<EntityId>.Enumerator enumerator = instance.GameAccount.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EntityId current = enumerator.get_Current();
						stream.WriteByte(26);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						EntityId.Serialize(stream, current);
					}
				}
			}
			if (instance.HasEmail)
			{
				stream.WriteByte(34);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Email));
			}
			if (instance.AvailableRegion.get_Count() > 0)
			{
				using (List<uint>.Enumerator enumerator2 = instance.AvailableRegion.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						stream.WriteByte(40);
						ProtocolParser.WriteUInt32(stream, current2);
					}
				}
			}
			if (instance.HasConnectedRegion)
			{
				stream.WriteByte(48);
				ProtocolParser.WriteUInt32(stream, instance.ConnectedRegion);
			}
			if (instance.HasBattleTag)
			{
				stream.WriteByte(58);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.BattleTag));
			}
			if (instance.HasGeoipCountry)
			{
				stream.WriteByte(66);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.GeoipCountry));
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += ProtocolParser.SizeOfUInt32(this.ErrorCode);
			if (this.HasAccount)
			{
				num += 1u;
				uint serializedSize = this.Account.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.GameAccount.get_Count() > 0)
			{
				using (List<EntityId>.Enumerator enumerator = this.GameAccount.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EntityId current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			if (this.HasEmail)
			{
				num += 1u;
				uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.Email);
				num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			}
			if (this.AvailableRegion.get_Count() > 0)
			{
				using (List<uint>.Enumerator enumerator2 = this.AvailableRegion.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += 1u;
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
			}
			if (this.HasConnectedRegion)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.ConnectedRegion);
			}
			if (this.HasBattleTag)
			{
				num += 1u;
				uint byteCount2 = (uint)Encoding.get_UTF8().GetByteCount(this.BattleTag);
				num += ProtocolParser.SizeOfUInt32(byteCount2) + byteCount2;
			}
			if (this.HasGeoipCountry)
			{
				num += 1u;
				uint byteCount3 = (uint)Encoding.get_UTF8().GetByteCount(this.GeoipCountry);
				num += ProtocolParser.SizeOfUInt32(byteCount3) + byteCount3;
			}
			num += 1u;
			return num;
		}

		public void SetErrorCode(uint val)
		{
			this.ErrorCode = val;
		}

		public void SetAccount(EntityId val)
		{
			this.Account = val;
		}

		public void AddGameAccount(EntityId val)
		{
			this._GameAccount.Add(val);
		}

		public void ClearGameAccount()
		{
			this._GameAccount.Clear();
		}

		public void SetGameAccount(List<EntityId> val)
		{
			this.GameAccount = val;
		}

		public void SetEmail(string val)
		{
			this.Email = val;
		}

		public void AddAvailableRegion(uint val)
		{
			this._AvailableRegion.Add(val);
		}

		public void ClearAvailableRegion()
		{
			this._AvailableRegion.Clear();
		}

		public void SetAvailableRegion(List<uint> val)
		{
			this.AvailableRegion = val;
		}

		public void SetConnectedRegion(uint val)
		{
			this.ConnectedRegion = val;
		}

		public void SetBattleTag(string val)
		{
			this.BattleTag = val;
		}

		public void SetGeoipCountry(string val)
		{
			this.GeoipCountry = val;
		}

		public void SetSessionKey(byte[] val)
		{
			this.SessionKey = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.ErrorCode.GetHashCode();
			if (this.HasAccount)
			{
				num ^= this.Account.GetHashCode();
			}
			using (List<EntityId>.Enumerator enumerator = this.GameAccount.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EntityId current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			if (this.HasEmail)
			{
				num ^= this.Email.GetHashCode();
			}
			using (List<uint>.Enumerator enumerator2 = this.AvailableRegion.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			if (this.HasConnectedRegion)
			{
				num ^= this.ConnectedRegion.GetHashCode();
			}
			if (this.HasBattleTag)
			{
				num ^= this.BattleTag.GetHashCode();
			}
			if (this.HasGeoipCountry)
			{
				num ^= this.GeoipCountry.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			LogonResult logonResult = obj as LogonResult;
			if (logonResult == null)
			{
				return false;
			}
			if (!this.ErrorCode.Equals(logonResult.ErrorCode))
			{
				return false;
			}
			if (this.HasAccount != logonResult.HasAccount || (this.HasAccount && !this.Account.Equals(logonResult.Account)))
			{
				return false;
			}
			if (this.GameAccount.get_Count() != logonResult.GameAccount.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.GameAccount.get_Count(); i++)
			{
				if (!this.GameAccount.get_Item(i).Equals(logonResult.GameAccount.get_Item(i)))
				{
					return false;
				}
			}
			if (this.HasEmail != logonResult.HasEmail || (this.HasEmail && !this.Email.Equals(logonResult.Email)))
			{
				return false;
			}
			if (this.AvailableRegion.get_Count() != logonResult.AvailableRegion.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.AvailableRegion.get_Count(); j++)
			{
				if (!this.AvailableRegion.get_Item(j).Equals(logonResult.AvailableRegion.get_Item(j)))
				{
					return false;
				}
			}
			return this.HasConnectedRegion == logonResult.HasConnectedRegion && (!this.HasConnectedRegion || this.ConnectedRegion.Equals(logonResult.ConnectedRegion)) && this.HasBattleTag == logonResult.HasBattleTag && (!this.HasBattleTag || this.BattleTag.Equals(logonResult.BattleTag)) && this.HasGeoipCountry == logonResult.HasGeoipCountry && (!this.HasGeoipCountry || this.GeoipCountry.Equals(logonResult.GeoipCountry));
		}

		public static LogonResult ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<LogonResult>(bs, 0, -1);
		}
	}
}
