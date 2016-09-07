using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol
{
	public class RoleSet : IProtoBuf
	{
		public bool HasSubtype;

		private string _Subtype;

		private List<Role> _Role = new List<Role>();

		private List<uint> _DefaultRole = new List<uint>();

		public bool HasMaxMembers;

		private int _MaxMembers;

		private List<Attribute> _Attribute = new List<Attribute>();

		public string Program
		{
			get;
			set;
		}

		public string Service
		{
			get;
			set;
		}

		public string Subtype
		{
			get
			{
				return this._Subtype;
			}
			set
			{
				this._Subtype = value;
				this.HasSubtype = (value != null);
			}
		}

		public List<Role> Role
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

		public List<Role> RoleList
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

		public List<uint> DefaultRole
		{
			get
			{
				return this._DefaultRole;
			}
			set
			{
				this._DefaultRole = value;
			}
		}

		public List<uint> DefaultRoleList
		{
			get
			{
				return this._DefaultRole;
			}
		}

		public int DefaultRoleCount
		{
			get
			{
				return this._DefaultRole.get_Count();
			}
		}

		public int MaxMembers
		{
			get
			{
				return this._MaxMembers;
			}
			set
			{
				this._MaxMembers = value;
				this.HasMaxMembers = true;
			}
		}

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

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			RoleSet.Deserialize(stream, this);
		}

		public static RoleSet Deserialize(Stream stream, RoleSet instance)
		{
			return RoleSet.Deserialize(stream, instance, -1L);
		}

		public static RoleSet DeserializeLengthDelimited(Stream stream)
		{
			RoleSet roleSet = new RoleSet();
			RoleSet.DeserializeLengthDelimited(stream, roleSet);
			return roleSet;
		}

		public static RoleSet DeserializeLengthDelimited(Stream stream, RoleSet instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return RoleSet.Deserialize(stream, instance, num);
		}

		public static RoleSet Deserialize(Stream stream, RoleSet instance, long limit)
		{
			instance.Subtype = "default";
			if (instance.Role == null)
			{
				instance.Role = new List<Role>();
			}
			if (instance.DefaultRole == null)
			{
				instance.DefaultRole = new List<uint>();
			}
			if (instance.Attribute == null)
			{
				instance.Attribute = new List<Attribute>();
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
										if (num2 != 48)
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
												instance.Attribute.Add(bnet.protocol.attribute.Attribute.DeserializeLengthDelimited(stream));
											}
										}
										else
										{
											instance.MaxMembers = (int)ProtocolParser.ReadUInt64(stream);
										}
									}
									else
									{
										long num3 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
										num3 += stream.get_Position();
										while (stream.get_Position() < num3)
										{
											instance.DefaultRole.Add(ProtocolParser.ReadUInt32(stream));
										}
										if (stream.get_Position() != num3)
										{
											throw new ProtocolBufferException("Read too many bytes in packed data");
										}
									}
								}
								else
								{
									instance.Role.Add(bnet.protocol.Role.DeserializeLengthDelimited(stream));
								}
							}
							else
							{
								instance.Subtype = ProtocolParser.ReadString(stream);
							}
						}
						else
						{
							instance.Service = ProtocolParser.ReadString(stream);
						}
					}
					else
					{
						instance.Program = ProtocolParser.ReadString(stream);
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
			RoleSet.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, RoleSet instance)
		{
			if (instance.Program == null)
			{
				throw new ArgumentNullException("Program", "Required by proto specification.");
			}
			stream.WriteByte(10);
			ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Program));
			if (instance.Service == null)
			{
				throw new ArgumentNullException("Service", "Required by proto specification.");
			}
			stream.WriteByte(18);
			ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Service));
			if (instance.HasSubtype)
			{
				stream.WriteByte(26);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Subtype));
			}
			if (instance.Role.get_Count() > 0)
			{
				using (List<Role>.Enumerator enumerator = instance.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Role current = enumerator.get_Current();
						stream.WriteByte(34);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.Role.Serialize(stream, current);
					}
				}
			}
			if (instance.DefaultRole.get_Count() > 0)
			{
				stream.WriteByte(42);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator2 = instance.DefaultRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator3 = instance.DefaultRole.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						uint current3 = enumerator3.get_Current();
						ProtocolParser.WriteUInt32(stream, current3);
					}
				}
			}
			if (instance.HasMaxMembers)
			{
				stream.WriteByte(48);
				ProtocolParser.WriteUInt64(stream, (ulong)((long)instance.MaxMembers));
			}
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator4 = instance.Attribute.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Attribute current4 = enumerator4.get_Current();
						stream.WriteByte(58);
						ProtocolParser.WriteUInt32(stream, current4.GetSerializedSize());
						bnet.protocol.attribute.Attribute.Serialize(stream, current4);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.Program);
			num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			uint byteCount2 = (uint)Encoding.get_UTF8().GetByteCount(this.Service);
			num += ProtocolParser.SizeOfUInt32(byteCount2) + byteCount2;
			if (this.HasSubtype)
			{
				num += 1u;
				uint byteCount3 = (uint)Encoding.get_UTF8().GetByteCount(this.Subtype);
				num += ProtocolParser.SizeOfUInt32(byteCount3) + byteCount3;
			}
			if (this.Role.get_Count() > 0)
			{
				using (List<Role>.Enumerator enumerator = this.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Role current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.DefaultRole.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator2 = this.DefaultRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			if (this.HasMaxMembers)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64((ulong)((long)this.MaxMembers));
			}
			if (this.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator3 = this.Attribute.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Attribute current3 = enumerator3.get_Current();
						num += 1u;
						uint serializedSize2 = current3.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			num += 2u;
			return num;
		}

		public void SetProgram(string val)
		{
			this.Program = val;
		}

		public void SetService(string val)
		{
			this.Service = val;
		}

		public void SetSubtype(string val)
		{
			this.Subtype = val;
		}

		public void AddRole(Role val)
		{
			this._Role.Add(val);
		}

		public void ClearRole()
		{
			this._Role.Clear();
		}

		public void SetRole(List<Role> val)
		{
			this.Role = val;
		}

		public void AddDefaultRole(uint val)
		{
			this._DefaultRole.Add(val);
		}

		public void ClearDefaultRole()
		{
			this._DefaultRole.Clear();
		}

		public void SetDefaultRole(List<uint> val)
		{
			this.DefaultRole = val;
		}

		public void SetMaxMembers(int val)
		{
			this.MaxMembers = val;
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

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.Program.GetHashCode();
			num ^= this.Service.GetHashCode();
			if (this.HasSubtype)
			{
				num ^= this.Subtype.GetHashCode();
			}
			using (List<Role>.Enumerator enumerator = this.Role.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Role current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<uint>.Enumerator enumerator2 = this.DefaultRole.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			if (this.HasMaxMembers)
			{
				num ^= this.MaxMembers.GetHashCode();
			}
			using (List<Attribute>.Enumerator enumerator3 = this.Attribute.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Attribute current3 = enumerator3.get_Current();
					num ^= current3.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			RoleSet roleSet = obj as RoleSet;
			if (roleSet == null)
			{
				return false;
			}
			if (!this.Program.Equals(roleSet.Program))
			{
				return false;
			}
			if (!this.Service.Equals(roleSet.Service))
			{
				return false;
			}
			if (this.HasSubtype != roleSet.HasSubtype || (this.HasSubtype && !this.Subtype.Equals(roleSet.Subtype)))
			{
				return false;
			}
			if (this.Role.get_Count() != roleSet.Role.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Role.get_Count(); i++)
			{
				if (!this.Role.get_Item(i).Equals(roleSet.Role.get_Item(i)))
				{
					return false;
				}
			}
			if (this.DefaultRole.get_Count() != roleSet.DefaultRole.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.DefaultRole.get_Count(); j++)
			{
				if (!this.DefaultRole.get_Item(j).Equals(roleSet.DefaultRole.get_Item(j)))
				{
					return false;
				}
			}
			if (this.HasMaxMembers != roleSet.HasMaxMembers || (this.HasMaxMembers && !this.MaxMembers.Equals(roleSet.MaxMembers)))
			{
				return false;
			}
			if (this.Attribute.get_Count() != roleSet.Attribute.get_Count())
			{
				return false;
			}
			for (int k = 0; k < this.Attribute.get_Count(); k++)
			{
				if (!this.Attribute.get_Item(k).Equals(roleSet.Attribute.get_Item(k)))
				{
					return false;
				}
			}
			return true;
		}

		public static RoleSet ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<RoleSet>(bs, 0, -1);
		}
	}
}
