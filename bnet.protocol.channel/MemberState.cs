using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel
{
	public class MemberState : IProtoBuf
	{
		private List<Attribute> _Attribute = new List<Attribute>();

		private List<uint> _Role = new List<uint>();

		public bool HasPrivileges;

		private ulong _Privileges;

		public bool HasInfo;

		private AccountInfo _Info;

		public bool HasHidden;

		private bool _Hidden;

		public List<Attribute> Attribute
		{
			get
			{
				return this._Attribute;
			}
			set
			{
				this._Attribute = value;
			}
		}

		public List<Attribute> AttributeList
		{
			get
			{
				return this._Attribute;
			}
		}

		public int AttributeCount
		{
			get
			{
				return this._Attribute.get_Count();
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

		public ulong Privileges
		{
			get
			{
				return this._Privileges;
			}
			set
			{
				this._Privileges = value;
				this.HasPrivileges = true;
			}
		}

		public AccountInfo Info
		{
			get
			{
				return this._Info;
			}
			set
			{
				this._Info = value;
				this.HasInfo = (value != null);
			}
		}

		public bool Hidden
		{
			get
			{
				return this._Hidden;
			}
			set
			{
				this._Hidden = value;
				this.HasHidden = true;
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
			MemberState.Deserialize(stream, this);
		}

		public static MemberState Deserialize(Stream stream, MemberState instance)
		{
			return MemberState.Deserialize(stream, instance, -1L);
		}

		public static MemberState DeserializeLengthDelimited(Stream stream)
		{
			MemberState memberState = new MemberState();
			MemberState.DeserializeLengthDelimited(stream, memberState);
			return memberState;
		}

		public static MemberState DeserializeLengthDelimited(Stream stream, MemberState instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return MemberState.Deserialize(stream, instance, num);
		}

		public static MemberState Deserialize(Stream stream, MemberState instance, long limit)
		{
			if (instance.Attribute == null)
			{
				instance.Attribute = new List<Attribute>();
			}
			if (instance.Role == null)
			{
				instance.Role = new List<uint>();
			}
			instance.Privileges = 0uL;
			instance.Hidden = false;
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
							if (num2 != 24)
							{
								if (num2 != 34)
								{
									if (num2 != 40)
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
										instance.Hidden = ProtocolParser.ReadBool(stream);
									}
								}
								else if (instance.Info == null)
								{
									instance.Info = AccountInfo.DeserializeLengthDelimited(stream);
								}
								else
								{
									AccountInfo.DeserializeLengthDelimited(stream, instance.Info);
								}
							}
							else
							{
								instance.Privileges = ProtocolParser.ReadUInt64(stream);
							}
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
						instance.Attribute.Add(bnet.protocol.attribute.Attribute.DeserializeLengthDelimited(stream));
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
			MemberState.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, MemberState instance)
		{
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = instance.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.attribute.Attribute.Serialize(stream, current);
					}
				}
			}
			if (instance.Role.get_Count() > 0)
			{
				stream.WriteByte(18);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator2 = instance.Role.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator3 = instance.Role.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						uint current3 = enumerator3.get_Current();
						ProtocolParser.WriteUInt32(stream, current3);
					}
				}
			}
			if (instance.HasPrivileges)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt64(stream, instance.Privileges);
			}
			if (instance.HasInfo)
			{
				stream.WriteByte(34);
				ProtocolParser.WriteUInt32(stream, instance.Info.GetSerializedSize());
				AccountInfo.Serialize(stream, instance.Info);
			}
			if (instance.HasHidden)
			{
				stream.WriteByte(40);
				ProtocolParser.WriteBool(stream, instance.Hidden);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.Role.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator2 = this.Role.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			if (this.HasPrivileges)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64(this.Privileges);
			}
			if (this.HasInfo)
			{
				num += 1u;
				uint serializedSize2 = this.Info.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
			if (this.HasHidden)
			{
				num += 1u;
				num += 1u;
			}
			return num;
		}

		public void AddAttribute(Attribute val)
		{
			this._Attribute.Add(val);
		}

		public void ClearAttribute()
		{
			this._Attribute.Clear();
		}

		public void SetAttribute(List<Attribute> val)
		{
			this.Attribute = val;
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

		public void SetPrivileges(ulong val)
		{
			this.Privileges = val;
		}

		public void SetInfo(AccountInfo val)
		{
			this.Info = val;
		}

		public void SetHidden(bool val)
		{
			this.Hidden = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<uint>.Enumerator enumerator2 = this.Role.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			if (this.HasPrivileges)
			{
				num ^= this.Privileges.GetHashCode();
			}
			if (this.HasInfo)
			{
				num ^= this.Info.GetHashCode();
			}
			if (this.HasHidden)
			{
				num ^= this.Hidden.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			MemberState memberState = obj as MemberState;
			if (memberState == null)
			{
				return false;
			}
			if (this.Attribute.get_Count() != memberState.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(memberState.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			if (this.Role.get_Count() != memberState.Role.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.Role.get_Count(); j++)
			{
				if (!this.Role.get_Item(j).Equals(memberState.Role.get_Item(j)))
				{
					return false;
				}
			}
			return this.HasPrivileges == memberState.HasPrivileges && (!this.HasPrivileges || this.Privileges.Equals(memberState.Privileges)) && this.HasInfo == memberState.HasInfo && (!this.HasInfo || this.Info.Equals(memberState.Info)) && this.HasHidden == memberState.HasHidden && (!this.HasHidden || this.Hidden.Equals(memberState.Hidden));
		}

		public static MemberState ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<MemberState>(bs, 0, -1);
		}
	}
}
