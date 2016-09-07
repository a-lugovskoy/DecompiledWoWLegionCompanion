using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.account
{
	public class AccountFieldTags : IProtoBuf
	{
		public bool HasAccountLevelInfoTag;

		private uint _AccountLevelInfoTag;

		public bool HasPrivacyInfoTag;

		private uint _PrivacyInfoTag;

		public bool HasParentalControlInfoTag;

		private uint _ParentalControlInfoTag;

		private List<ProgramTag> _GameLevelInfoTags = new List<ProgramTag>();

		private List<ProgramTag> _GameStatusTags = new List<ProgramTag>();

		private List<RegionTag> _GameAccountTags = new List<RegionTag>();

		public uint AccountLevelInfoTag
		{
			get
			{
				return this._AccountLevelInfoTag;
			}
			set
			{
				this._AccountLevelInfoTag = value;
				this.HasAccountLevelInfoTag = true;
			}
		}

		public uint PrivacyInfoTag
		{
			get
			{
				return this._PrivacyInfoTag;
			}
			set
			{
				this._PrivacyInfoTag = value;
				this.HasPrivacyInfoTag = true;
			}
		}

		public uint ParentalControlInfoTag
		{
			get
			{
				return this._ParentalControlInfoTag;
			}
			set
			{
				this._ParentalControlInfoTag = value;
				this.HasParentalControlInfoTag = true;
			}
		}

		public List<ProgramTag> GameLevelInfoTags
		{
			get
			{
				return this._GameLevelInfoTags;
			}
			set
			{
				this._GameLevelInfoTags = value;
			}
		}

		public List<ProgramTag> GameLevelInfoTagsList
		{
			get
			{
				return this._GameLevelInfoTags;
			}
		}

		public int GameLevelInfoTagsCount
		{
			get
			{
				return this._GameLevelInfoTags.get_Count();
			}
		}

		public List<ProgramTag> GameStatusTags
		{
			get
			{
				return this._GameStatusTags;
			}
			set
			{
				this._GameStatusTags = value;
			}
		}

		public List<ProgramTag> GameStatusTagsList
		{
			get
			{
				return this._GameStatusTags;
			}
		}

		public int GameStatusTagsCount
		{
			get
			{
				return this._GameStatusTags.get_Count();
			}
		}

		public List<RegionTag> GameAccountTags
		{
			get
			{
				return this._GameAccountTags;
			}
			set
			{
				this._GameAccountTags = value;
			}
		}

		public List<RegionTag> GameAccountTagsList
		{
			get
			{
				return this._GameAccountTags;
			}
		}

		public int GameAccountTagsCount
		{
			get
			{
				return this._GameAccountTags.get_Count();
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
			AccountFieldTags.Deserialize(stream, this);
		}

		public static AccountFieldTags Deserialize(Stream stream, AccountFieldTags instance)
		{
			return AccountFieldTags.Deserialize(stream, instance, -1L);
		}

		public static AccountFieldTags DeserializeLengthDelimited(Stream stream)
		{
			AccountFieldTags accountFieldTags = new AccountFieldTags();
			AccountFieldTags.DeserializeLengthDelimited(stream, accountFieldTags);
			return accountFieldTags;
		}

		public static AccountFieldTags DeserializeLengthDelimited(Stream stream, AccountFieldTags instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return AccountFieldTags.Deserialize(stream, instance, num);
		}

		public static AccountFieldTags Deserialize(Stream stream, AccountFieldTags instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.GameLevelInfoTags == null)
			{
				instance.GameLevelInfoTags = new List<ProgramTag>();
			}
			if (instance.GameStatusTags == null)
			{
				instance.GameStatusTags = new List<ProgramTag>();
			}
			if (instance.GameAccountTags == null)
			{
				instance.GameAccountTags = new List<RegionTag>();
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
					if (num2 != 21)
					{
						if (num2 != 29)
						{
							if (num2 != 37)
							{
								if (num2 != 58)
								{
									if (num2 != 74)
									{
										if (num2 != 90)
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
											instance.GameAccountTags.Add(RegionTag.DeserializeLengthDelimited(stream));
										}
									}
									else
									{
										instance.GameStatusTags.Add(ProgramTag.DeserializeLengthDelimited(stream));
									}
								}
								else
								{
									instance.GameLevelInfoTags.Add(ProgramTag.DeserializeLengthDelimited(stream));
								}
							}
							else
							{
								instance.ParentalControlInfoTag = binaryReader.ReadUInt32();
							}
						}
						else
						{
							instance.PrivacyInfoTag = binaryReader.ReadUInt32();
						}
					}
					else
					{
						instance.AccountLevelInfoTag = binaryReader.ReadUInt32();
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
			AccountFieldTags.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, AccountFieldTags instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.HasAccountLevelInfoTag)
			{
				stream.WriteByte(21);
				binaryWriter.Write(instance.AccountLevelInfoTag);
			}
			if (instance.HasPrivacyInfoTag)
			{
				stream.WriteByte(29);
				binaryWriter.Write(instance.PrivacyInfoTag);
			}
			if (instance.HasParentalControlInfoTag)
			{
				stream.WriteByte(37);
				binaryWriter.Write(instance.ParentalControlInfoTag);
			}
			if (instance.GameLevelInfoTags.get_Count() > 0)
			{
				using (List<ProgramTag>.Enumerator enumerator = instance.GameLevelInfoTags.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ProgramTag current = enumerator.get_Current();
						stream.WriteByte(58);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						ProgramTag.Serialize(stream, current);
					}
				}
			}
			if (instance.GameStatusTags.get_Count() > 0)
			{
				using (List<ProgramTag>.Enumerator enumerator2 = instance.GameStatusTags.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ProgramTag current2 = enumerator2.get_Current();
						stream.WriteByte(74);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						ProgramTag.Serialize(stream, current2);
					}
				}
			}
			if (instance.GameAccountTags.get_Count() > 0)
			{
				using (List<RegionTag>.Enumerator enumerator3 = instance.GameAccountTags.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						RegionTag current3 = enumerator3.get_Current();
						stream.WriteByte(90);
						ProtocolParser.WriteUInt32(stream, current3.GetSerializedSize());
						RegionTag.Serialize(stream, current3);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasAccountLevelInfoTag)
			{
				num += 1u;
				num += 4u;
			}
			if (this.HasPrivacyInfoTag)
			{
				num += 1u;
				num += 4u;
			}
			if (this.HasParentalControlInfoTag)
			{
				num += 1u;
				num += 4u;
			}
			if (this.GameLevelInfoTags.get_Count() > 0)
			{
				using (List<ProgramTag>.Enumerator enumerator = this.GameLevelInfoTags.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ProgramTag current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.GameStatusTags.get_Count() > 0)
			{
				using (List<ProgramTag>.Enumerator enumerator2 = this.GameStatusTags.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ProgramTag current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize2 = current2.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			if (this.GameAccountTags.get_Count() > 0)
			{
				using (List<RegionTag>.Enumerator enumerator3 = this.GameAccountTags.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						RegionTag current3 = enumerator3.get_Current();
						num += 1u;
						uint serializedSize3 = current3.GetSerializedSize();
						num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
					}
				}
			}
			return num;
		}

		public void SetAccountLevelInfoTag(uint val)
		{
			this.AccountLevelInfoTag = val;
		}

		public void SetPrivacyInfoTag(uint val)
		{
			this.PrivacyInfoTag = val;
		}

		public void SetParentalControlInfoTag(uint val)
		{
			this.ParentalControlInfoTag = val;
		}

		public void AddGameLevelInfoTags(ProgramTag val)
		{
			this._GameLevelInfoTags.Add(val);
		}

		public void ClearGameLevelInfoTags()
		{
			this._GameLevelInfoTags.Clear();
		}

		public void SetGameLevelInfoTags(List<ProgramTag> val)
		{
			this.GameLevelInfoTags = val;
		}

		public void AddGameStatusTags(ProgramTag val)
		{
			this._GameStatusTags.Add(val);
		}

		public void ClearGameStatusTags()
		{
			this._GameStatusTags.Clear();
		}

		public void SetGameStatusTags(List<ProgramTag> val)
		{
			this.GameStatusTags = val;
		}

		public void AddGameAccountTags(RegionTag val)
		{
			this._GameAccountTags.Add(val);
		}

		public void ClearGameAccountTags()
		{
			this._GameAccountTags.Clear();
		}

		public void SetGameAccountTags(List<RegionTag> val)
		{
			this.GameAccountTags = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasAccountLevelInfoTag)
			{
				num ^= this.AccountLevelInfoTag.GetHashCode();
			}
			if (this.HasPrivacyInfoTag)
			{
				num ^= this.PrivacyInfoTag.GetHashCode();
			}
			if (this.HasParentalControlInfoTag)
			{
				num ^= this.ParentalControlInfoTag.GetHashCode();
			}
			using (List<ProgramTag>.Enumerator enumerator = this.GameLevelInfoTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProgramTag current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<ProgramTag>.Enumerator enumerator2 = this.GameStatusTags.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ProgramTag current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			using (List<RegionTag>.Enumerator enumerator3 = this.GameAccountTags.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					RegionTag current3 = enumerator3.get_Current();
					num ^= current3.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			AccountFieldTags accountFieldTags = obj as AccountFieldTags;
			if (accountFieldTags == null)
			{
				return false;
			}
			if (this.HasAccountLevelInfoTag != accountFieldTags.HasAccountLevelInfoTag || (this.HasAccountLevelInfoTag && !this.AccountLevelInfoTag.Equals(accountFieldTags.AccountLevelInfoTag)))
			{
				return false;
			}
			if (this.HasPrivacyInfoTag != accountFieldTags.HasPrivacyInfoTag || (this.HasPrivacyInfoTag && !this.PrivacyInfoTag.Equals(accountFieldTags.PrivacyInfoTag)))
			{
				return false;
			}
			if (this.HasParentalControlInfoTag != accountFieldTags.HasParentalControlInfoTag || (this.HasParentalControlInfoTag && !this.ParentalControlInfoTag.Equals(accountFieldTags.ParentalControlInfoTag)))
			{
				return false;
			}
			if (this.GameLevelInfoTags.get_Count() != accountFieldTags.GameLevelInfoTags.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.GameLevelInfoTags.get_Count(); i++)
			{
				if (!this.GameLevelInfoTags.get_Item(i).Equals(accountFieldTags.GameLevelInfoTags.get_Item(i)))
				{
					return false;
				}
			}
			if (this.GameStatusTags.get_Count() != accountFieldTags.GameStatusTags.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.GameStatusTags.get_Count(); j++)
			{
				if (!this.GameStatusTags.get_Item(j).Equals(accountFieldTags.GameStatusTags.get_Item(j)))
				{
					return false;
				}
			}
			if (this.GameAccountTags.get_Count() != accountFieldTags.GameAccountTags.get_Count())
			{
				return false;
			}
			for (int k = 0; k < this.GameAccountTags.get_Count(); k++)
			{
				if (!this.GameAccountTags.get_Item(k).Equals(accountFieldTags.GameAccountTags.get_Item(k)))
				{
					return false;
				}
			}
			return true;
		}

		public static AccountFieldTags ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<AccountFieldTags>(bs, 0, -1);
		}
	}
}
