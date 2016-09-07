using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol.friends
{
	public class FriendInvitationParams : IProtoBuf
	{
		public bool HasTargetEmail;

		private string _TargetEmail;

		public bool HasTargetBattleTag;

		private string _TargetBattleTag;

		public bool HasInviterBattleTag;

		private string _InviterBattleTag;

		public bool HasInviterFullName;

		private string _InviterFullName;

		public bool HasInviteeDisplayName;

		private string _InviteeDisplayName;

		private List<uint> _Role = new List<uint>();

		public string TargetEmail
		{
			get
			{
				return this._TargetEmail;
			}
			set
			{
				this._TargetEmail = value;
				this.HasTargetEmail = (value != null);
			}
		}

		public string TargetBattleTag
		{
			get
			{
				return this._TargetBattleTag;
			}
			set
			{
				this._TargetBattleTag = value;
				this.HasTargetBattleTag = (value != null);
			}
		}

		public string InviterBattleTag
		{
			get
			{
				return this._InviterBattleTag;
			}
			set
			{
				this._InviterBattleTag = value;
				this.HasInviterBattleTag = (value != null);
			}
		}

		public string InviterFullName
		{
			get
			{
				return this._InviterFullName;
			}
			set
			{
				this._InviterFullName = value;
				this.HasInviterFullName = (value != null);
			}
		}

		public string InviteeDisplayName
		{
			get
			{
				return this._InviteeDisplayName;
			}
			set
			{
				this._InviteeDisplayName = value;
				this.HasInviteeDisplayName = (value != null);
			}
		}

		public List<uint> Role
		{
			get
			{
				return this._Role;
			}
			set
			{
				this._Role = value;
			}
		}

		public List<uint> RoleList
		{
			get
			{
				return this._Role;
			}
		}

		public int RoleCount
		{
			get
			{
				return this._Role.get_Count();
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
			FriendInvitationParams.Deserialize(stream, this);
		}

		public static FriendInvitationParams Deserialize(Stream stream, FriendInvitationParams instance)
		{
			return FriendInvitationParams.Deserialize(stream, instance, -1L);
		}

		public static FriendInvitationParams DeserializeLengthDelimited(Stream stream)
		{
			FriendInvitationParams friendInvitationParams = new FriendInvitationParams();
			FriendInvitationParams.DeserializeLengthDelimited(stream, friendInvitationParams);
			return friendInvitationParams;
		}

		public static FriendInvitationParams DeserializeLengthDelimited(Stream stream, FriendInvitationParams instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return FriendInvitationParams.Deserialize(stream, instance, num);
		}

		public static FriendInvitationParams Deserialize(Stream stream, FriendInvitationParams instance, long limit)
		{
			if (instance.Role == null)
			{
				instance.Role = new List<uint>();
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
								if (num2 != 34)
								{
									if (num2 != 42)
									{
										if (num2 != 50)
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
											long num3 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
											num3 += stream.get_Position();
											while (stream.get_Position() < num3)
											{
												instance.Role.Add(ProtocolParser.ReadUInt32(stream));
											}
											if (stream.get_Position() != num3)
											{
												throw new ProtocolBufferException("Read too many bytes in packed data");
											}
										}
									}
									else
									{
										instance.InviteeDisplayName = ProtocolParser.ReadString(stream);
									}
								}
								else
								{
									instance.InviterFullName = ProtocolParser.ReadString(stream);
								}
							}
							else
							{
								instance.InviterBattleTag = ProtocolParser.ReadString(stream);
							}
						}
						else
						{
							instance.TargetBattleTag = ProtocolParser.ReadString(stream);
						}
					}
					else
					{
						instance.TargetEmail = ProtocolParser.ReadString(stream);
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
			FriendInvitationParams.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, FriendInvitationParams instance)
		{
			if (instance.HasTargetEmail)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.TargetEmail));
			}
			if (instance.HasTargetBattleTag)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.TargetBattleTag));
			}
			if (instance.HasInviterBattleTag)
			{
				stream.WriteByte(26);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.InviterBattleTag));
			}
			if (instance.HasInviterFullName)
			{
				stream.WriteByte(34);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.InviterFullName));
			}
			if (instance.HasInviteeDisplayName)
			{
				stream.WriteByte(42);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.InviteeDisplayName));
			}
			if (instance.Role.get_Count() > 0)
			{
				stream.WriteByte(50);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator = instance.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator2 = instance.Role.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						ProtocolParser.WriteUInt32(stream, current2);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasTargetEmail)
			{
				num += 1u;
				uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.TargetEmail);
				num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			}
			if (this.HasTargetBattleTag)
			{
				num += 1u;
				uint byteCount2 = (uint)Encoding.get_UTF8().GetByteCount(this.TargetBattleTag);
				num += ProtocolParser.SizeOfUInt32(byteCount2) + byteCount2;
			}
			if (this.HasInviterBattleTag)
			{
				num += 1u;
				uint byteCount3 = (uint)Encoding.get_UTF8().GetByteCount(this.InviterBattleTag);
				num += ProtocolParser.SizeOfUInt32(byteCount3) + byteCount3;
			}
			if (this.HasInviterFullName)
			{
				num += 1u;
				uint byteCount4 = (uint)Encoding.get_UTF8().GetByteCount(this.InviterFullName);
				num += ProtocolParser.SizeOfUInt32(byteCount4) + byteCount4;
			}
			if (this.HasInviteeDisplayName)
			{
				num += 1u;
				uint byteCount5 = (uint)Encoding.get_UTF8().GetByteCount(this.InviteeDisplayName);
				num += ProtocolParser.SizeOfUInt32(byteCount5) + byteCount5;
			}
			if (this.Role.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator = this.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			return num;
		}

		public void SetTargetEmail(string val)
		{
			this.TargetEmail = val;
		}

		public void SetTargetBattleTag(string val)
		{
			this.TargetBattleTag = val;
		}

		public void SetInviterBattleTag(string val)
		{
			this.InviterBattleTag = val;
		}

		public void SetInviterFullName(string val)
		{
			this.InviterFullName = val;
		}

		public void SetInviteeDisplayName(string val)
		{
			this.InviteeDisplayName = val;
		}

		public void AddRole(uint val)
		{
			this._Role.Add(val);
		}

		public void ClearRole()
		{
			this._Role.Clear();
		}

		public void SetRole(List<uint> val)
		{
			this.Role = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasTargetEmail)
			{
				num ^= this.TargetEmail.GetHashCode();
			}
			if (this.HasTargetBattleTag)
			{
				num ^= this.TargetBattleTag.GetHashCode();
			}
			if (this.HasInviterBattleTag)
			{
				num ^= this.InviterBattleTag.GetHashCode();
			}
			if (this.HasInviterFullName)
			{
				num ^= this.InviterFullName.GetHashCode();
			}
			if (this.HasInviteeDisplayName)
			{
				num ^= this.InviteeDisplayName.GetHashCode();
			}
			using (List<uint>.Enumerator enumerator = this.Role.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			FriendInvitationParams friendInvitationParams = obj as FriendInvitationParams;
			if (friendInvitationParams == null)
			{
				return false;
			}
			if (this.HasTargetEmail != friendInvitationParams.HasTargetEmail || (this.HasTargetEmail && !this.TargetEmail.Equals(friendInvitationParams.TargetEmail)))
			{
				return false;
			}
			if (this.HasTargetBattleTag != friendInvitationParams.HasTargetBattleTag || (this.HasTargetBattleTag && !this.TargetBattleTag.Equals(friendInvitationParams.TargetBattleTag)))
			{
				return false;
			}
			if (this.HasInviterBattleTag != friendInvitationParams.HasInviterBattleTag || (this.HasInviterBattleTag && !this.InviterBattleTag.Equals(friendInvitationParams.InviterBattleTag)))
			{
				return false;
			}
			if (this.HasInviterFullName != friendInvitationParams.HasInviterFullName || (this.HasInviterFullName && !this.InviterFullName.Equals(friendInvitationParams.InviterFullName)))
			{
				return false;
			}
			if (this.HasInviteeDisplayName != friendInvitationParams.HasInviteeDisplayName || (this.HasInviteeDisplayName && !this.InviteeDisplayName.Equals(friendInvitationParams.InviteeDisplayName)))
			{
				return false;
			}
			if (this.Role.get_Count() != friendInvitationParams.Role.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Role.get_Count(); i++)
			{
				if (!this.Role.get_Item(i).Equals(friendInvitationParams.Role.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static FriendInvitationParams ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<FriendInvitationParams>(bs, 0, -1);
		}
	}
}
