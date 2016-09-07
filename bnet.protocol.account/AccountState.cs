using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.account
{
	public class AccountState : IProtoBuf
	{
		public bool HasAccountLevelInfo;

		private AccountLevelInfo _AccountLevelInfo;

		public bool HasPrivacyInfo;

		private PrivacyInfo _PrivacyInfo;

		public bool HasParentalControlInfo;

		private ParentalControlInfo _ParentalControlInfo;

		private List<GameLevelInfo> _GameLevelInfo = new List<GameLevelInfo>();

		private List<GameStatus> _GameStatus = new List<GameStatus>();

		private List<GameAccountList> _GameAccounts = new List<GameAccountList>();

		public AccountLevelInfo AccountLevelInfo
		{
			get
			{
				return this._AccountLevelInfo;
			}
			set
			{
				this._AccountLevelInfo = value;
				this.HasAccountLevelInfo = (value != null);
			}
		}

		public PrivacyInfo PrivacyInfo
		{
			get
			{
				return this._PrivacyInfo;
			}
			set
			{
				this._PrivacyInfo = value;
				this.HasPrivacyInfo = (value != null);
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

		public List<GameLevelInfo> GameLevelInfo
		{
			get
			{
				return this._GameLevelInfo;
			}
			set
			{
				this._GameLevelInfo = value;
			}
		}

		public List<GameLevelInfo> GameLevelInfoList
		{
			get
			{
				return this._GameLevelInfo;
			}
		}

		public int GameLevelInfoCount
		{
			get
			{
				return this._GameLevelInfo.get_Count();
			}
		}

		public List<GameStatus> GameStatus
		{
			get
			{
				return this._GameStatus;
			}
			set
			{
				this._GameStatus = value;
			}
		}

		public List<GameStatus> GameStatusList
		{
			get
			{
				return this._GameStatus;
			}
		}

		public int GameStatusCount
		{
			get
			{
				return this._GameStatus.get_Count();
			}
		}

		public List<GameAccountList> GameAccounts
		{
			get
			{
				return this._GameAccounts;
			}
			set
			{
				this._GameAccounts = value;
			}
		}

		public List<GameAccountList> GameAccountsList
		{
			get
			{
				return this._GameAccounts;
			}
		}

		public int GameAccountsCount
		{
			get
			{
				return this._GameAccounts.get_Count();
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
			AccountState.Deserialize(stream, this);
		}

		public static AccountState Deserialize(Stream stream, AccountState instance)
		{
			return AccountState.Deserialize(stream, instance, -1L);
		}

		public static AccountState DeserializeLengthDelimited(Stream stream)
		{
			AccountState accountState = new AccountState();
			AccountState.DeserializeLengthDelimited(stream, accountState);
			return accountState;
		}

		public static AccountState DeserializeLengthDelimited(Stream stream, AccountState instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return AccountState.Deserialize(stream, instance, num);
		}

		public static AccountState Deserialize(Stream stream, AccountState instance, long limit)
		{
			if (instance.GameLevelInfo == null)
			{
				instance.GameLevelInfo = new List<GameLevelInfo>();
			}
			if (instance.GameStatus == null)
			{
				instance.GameStatus = new List<GameStatus>();
			}
			if (instance.GameAccounts == null)
			{
				instance.GameAccounts = new List<GameAccountList>();
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
						if (num2 != 18)
						{
							if (num2 != 26)
							{
								if (num2 != 42)
								{
									if (num2 != 50)
									{
										if (num2 != 58)
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
											instance.GameAccounts.Add(GameAccountList.DeserializeLengthDelimited(stream));
										}
									}
									else
									{
										instance.GameStatus.Add(bnet.protocol.account.GameStatus.DeserializeLengthDelimited(stream));
									}
								}
								else
								{
									instance.GameLevelInfo.Add(bnet.protocol.account.GameLevelInfo.DeserializeLengthDelimited(stream));
								}
							}
							else if (instance.ParentalControlInfo == null)
							{
								instance.ParentalControlInfo = ParentalControlInfo.DeserializeLengthDelimited(stream);
							}
							else
							{
								ParentalControlInfo.DeserializeLengthDelimited(stream, instance.ParentalControlInfo);
							}
						}
						else if (instance.PrivacyInfo == null)
						{
							instance.PrivacyInfo = PrivacyInfo.DeserializeLengthDelimited(stream);
						}
						else
						{
							PrivacyInfo.DeserializeLengthDelimited(stream, instance.PrivacyInfo);
						}
					}
					else if (instance.AccountLevelInfo == null)
					{
						instance.AccountLevelInfo = AccountLevelInfo.DeserializeLengthDelimited(stream);
					}
					else
					{
						AccountLevelInfo.DeserializeLengthDelimited(stream, instance.AccountLevelInfo);
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
			AccountState.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, AccountState instance)
		{
			if (instance.HasAccountLevelInfo)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.AccountLevelInfo.GetSerializedSize());
				AccountLevelInfo.Serialize(stream, instance.AccountLevelInfo);
			}
			if (instance.HasPrivacyInfo)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteUInt32(stream, instance.PrivacyInfo.GetSerializedSize());
				PrivacyInfo.Serialize(stream, instance.PrivacyInfo);
			}
			if (instance.HasParentalControlInfo)
			{
				stream.WriteByte(26);
				ProtocolParser.WriteUInt32(stream, instance.ParentalControlInfo.GetSerializedSize());
				ParentalControlInfo.Serialize(stream, instance.ParentalControlInfo);
			}
			if (instance.GameLevelInfo.get_Count() > 0)
			{
				using (List<GameLevelInfo>.Enumerator enumerator = instance.GameLevelInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameLevelInfo current = enumerator.get_Current();
						stream.WriteByte(42);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.account.GameLevelInfo.Serialize(stream, current);
					}
				}
			}
			if (instance.GameStatus.get_Count() > 0)
			{
				using (List<GameStatus>.Enumerator enumerator2 = instance.GameStatus.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameStatus current2 = enumerator2.get_Current();
						stream.WriteByte(50);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						bnet.protocol.account.GameStatus.Serialize(stream, current2);
					}
				}
			}
			if (instance.GameAccounts.get_Count() > 0)
			{
				using (List<GameAccountList>.Enumerator enumerator3 = instance.GameAccounts.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						GameAccountList current3 = enumerator3.get_Current();
						stream.WriteByte(58);
						ProtocolParser.WriteUInt32(stream, current3.GetSerializedSize());
						GameAccountList.Serialize(stream, current3);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasAccountLevelInfo)
			{
				num += 1u;
				uint serializedSize = this.AccountLevelInfo.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.HasPrivacyInfo)
			{
				num += 1u;
				uint serializedSize2 = this.PrivacyInfo.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
			if (this.HasParentalControlInfo)
			{
				num += 1u;
				uint serializedSize3 = this.ParentalControlInfo.GetSerializedSize();
				num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
			}
			if (this.GameLevelInfo.get_Count() > 0)
			{
				using (List<GameLevelInfo>.Enumerator enumerator = this.GameLevelInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameLevelInfo current = enumerator.get_Current();
						num += 1u;
						uint serializedSize4 = current.GetSerializedSize();
						num += serializedSize4 + ProtocolParser.SizeOfUInt32(serializedSize4);
					}
				}
			}
			if (this.GameStatus.get_Count() > 0)
			{
				using (List<GameStatus>.Enumerator enumerator2 = this.GameStatus.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameStatus current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize5 = current2.GetSerializedSize();
						num += serializedSize5 + ProtocolParser.SizeOfUInt32(serializedSize5);
					}
				}
			}
			if (this.GameAccounts.get_Count() > 0)
			{
				using (List<GameAccountList>.Enumerator enumerator3 = this.GameAccounts.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						GameAccountList current3 = enumerator3.get_Current();
						num += 1u;
						uint serializedSize6 = current3.GetSerializedSize();
						num += serializedSize6 + ProtocolParser.SizeOfUInt32(serializedSize6);
					}
				}
			}
			return num;
		}

		public void SetAccountLevelInfo(AccountLevelInfo val)
		{
			this.AccountLevelInfo = val;
		}

		public void SetPrivacyInfo(PrivacyInfo val)
		{
			this.PrivacyInfo = val;
		}

		public void SetParentalControlInfo(ParentalControlInfo val)
		{
			this.ParentalControlInfo = val;
		}

		public void AddGameLevelInfo(GameLevelInfo val)
		{
			this._GameLevelInfo.Add(val);
		}

		public void ClearGameLevelInfo()
		{
			this._GameLevelInfo.Clear();
		}

		public void SetGameLevelInfo(List<GameLevelInfo> val)
		{
			this.GameLevelInfo = val;
		}

		public void AddGameStatus(GameStatus val)
		{
			this._GameStatus.Add(val);
		}

		public void ClearGameStatus()
		{
			this._GameStatus.Clear();
		}

		public void SetGameStatus(List<GameStatus> val)
		{
			this.GameStatus = val;
		}

		public void AddGameAccounts(GameAccountList val)
		{
			this._GameAccounts.Add(val);
		}

		public void ClearGameAccounts()
		{
			this._GameAccounts.Clear();
		}

		public void SetGameAccounts(List<GameAccountList> val)
		{
			this.GameAccounts = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasAccountLevelInfo)
			{
				num ^= this.AccountLevelInfo.GetHashCode();
			}
			if (this.HasPrivacyInfo)
			{
				num ^= this.PrivacyInfo.GetHashCode();
			}
			if (this.HasParentalControlInfo)
			{
				num ^= this.ParentalControlInfo.GetHashCode();
			}
			using (List<GameLevelInfo>.Enumerator enumerator = this.GameLevelInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameLevelInfo current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<GameStatus>.Enumerator enumerator2 = this.GameStatus.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameStatus current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			using (List<GameAccountList>.Enumerator enumerator3 = this.GameAccounts.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					GameAccountList current3 = enumerator3.get_Current();
					num ^= current3.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			AccountState accountState = obj as AccountState;
			if (accountState == null)
			{
				return false;
			}
			if (this.HasAccountLevelInfo != accountState.HasAccountLevelInfo || (this.HasAccountLevelInfo && !this.AccountLevelInfo.Equals(accountState.AccountLevelInfo)))
			{
				return false;
			}
			if (this.HasPrivacyInfo != accountState.HasPrivacyInfo || (this.HasPrivacyInfo && !this.PrivacyInfo.Equals(accountState.PrivacyInfo)))
			{
				return false;
			}
			if (this.HasParentalControlInfo != accountState.HasParentalControlInfo || (this.HasParentalControlInfo && !this.ParentalControlInfo.Equals(accountState.ParentalControlInfo)))
			{
				return false;
			}
			if (this.GameLevelInfo.get_Count() != accountState.GameLevelInfo.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.GameLevelInfo.get_Count(); i++)
			{
				if (!this.GameLevelInfo.get_Item(i).Equals(accountState.GameLevelInfo.get_Item(i)))
				{
					return false;
				}
			}
			if (this.GameStatus.get_Count() != accountState.GameStatus.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.GameStatus.get_Count(); j++)
			{
				if (!this.GameStatus.get_Item(j).Equals(accountState.GameStatus.get_Item(j)))
				{
					return false;
				}
			}
			if (this.GameAccounts.get_Count() != accountState.GameAccounts.get_Count())
			{
				return false;
			}
			for (int k = 0; k < this.GameAccounts.get_Count(); k++)
			{
				if (!this.GameAccounts.get_Item(k).Equals(accountState.GameAccounts.get_Item(k)))
				{
					return false;
				}
			}
			return true;
		}

		public static AccountState ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<AccountState>(bs, 0, -1);
		}
	}
}
