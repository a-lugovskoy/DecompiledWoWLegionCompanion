using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol
{
	public class Role : IProtoBuf
	{
		private List<string> _Privilege = new List<string>();

		private List<uint> _AssignableRole = new List<uint>();

		public bool HasRequired;

		private bool _Required;

		public bool HasUnique;

		private bool _Unique;

		public bool HasRelegationRole;

		private uint _RelegationRole;

		private List<Attribute> _Attribute = new List<Attribute>();

		public uint Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public List<string> Privilege
		{
			get
			{
				return this._Privilege;
			}
			set
			{
				this._Privilege = value;
			}
		}

		public List<string> PrivilegeList
		{
			get
			{
				return this._Privilege;
			}
		}

		public int PrivilegeCount
		{
			get
			{
				return this._Privilege.get_Count();
			}
		}

		public List<uint> AssignableRole
		{
			get
			{
				return this._AssignableRole;
			}
			set
			{
				this._AssignableRole = value;
			}
		}

		public List<uint> AssignableRoleList
		{
			get
			{
				return this._AssignableRole;
			}
		}

		public int AssignableRoleCount
		{
			get
			{
				return this._AssignableRole.get_Count();
			}
		}

		public bool Required
		{
			get
			{
				return this._Required;
			}
			set
			{
				this._Required = value;
				this.HasRequired = true;
			}
		}

		public bool Unique
		{
			get
			{
				return this._Unique;
			}
			set
			{
				this._Unique = value;
				this.HasUnique = true;
			}
		}

		public uint RelegationRole
		{
			get
			{
				return this._RelegationRole;
			}
			set
			{
				this._RelegationRole = value;
				this.HasRelegationRole = true;
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
			Role.Deserialize(stream, this);
		}

		public static Role Deserialize(Stream stream, Role instance)
		{
			return Role.Deserialize(stream, instance, -1L);
		}

		public static Role DeserializeLengthDelimited(Stream stream)
		{
			Role role = new Role();
			Role.DeserializeLengthDelimited(stream, role);
			return role;
		}

		public static Role DeserializeLengthDelimited(Stream stream, Role instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return Role.Deserialize(stream, instance, num);
		}

		public static Role Deserialize(Stream stream, Role instance, long limit)
		{
			if (instance.Privilege == null)
			{
				instance.Privilege = new List<string>();
			}
			if (instance.AssignableRole == null)
			{
				instance.AssignableRole = new List<uint>();
			}
			instance.Required = false;
			instance.Unique = false;
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
											if (num2 != 56)
											{
												if (num2 != 66)
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
												instance.RelegationRole = ProtocolParser.ReadUInt32(stream);
											}
										}
										else
										{
											instance.Unique = ProtocolParser.ReadBool(stream);
										}
									}
									else
									{
										instance.Required = ProtocolParser.ReadBool(stream);
									}
								}
								else
								{
									long num3 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
									num3 += stream.get_Position();
									while (stream.get_Position() < num3)
									{
										instance.AssignableRole.Add(ProtocolParser.ReadUInt32(stream));
									}
									if (stream.get_Position() != num3)
									{
										throw new ProtocolBufferException("Read too many bytes in packed data");
									}
								}
							}
							else
							{
								instance.Privilege.Add(ProtocolParser.ReadString(stream));
							}
						}
						else
						{
							instance.Name = ProtocolParser.ReadString(stream);
						}
					}
					else
					{
						instance.Id = ProtocolParser.ReadUInt32(stream);
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
			Role.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, Role instance)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.Id);
			if (instance.Name == null)
			{
				throw new ArgumentNullException("Name", "Required by proto specification.");
			}
			stream.WriteByte(18);
			ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Name));
			if (instance.Privilege.get_Count() > 0)
			{
				using (List<string>.Enumerator enumerator = instance.Privilege.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						stream.WriteByte(26);
						ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(current));
					}
				}
			}
			if (instance.AssignableRole.get_Count() > 0)
			{
				stream.WriteByte(34);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator2 = instance.AssignableRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator3 = instance.AssignableRole.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						uint current3 = enumerator3.get_Current();
						ProtocolParser.WriteUInt32(stream, current3);
					}
				}
			}
			if (instance.HasRequired)
			{
				stream.WriteByte(40);
				ProtocolParser.WriteBool(stream, instance.Required);
			}
			if (instance.HasUnique)
			{
				stream.WriteByte(48);
				ProtocolParser.WriteBool(stream, instance.Unique);
			}
			if (instance.HasRelegationRole)
			{
				stream.WriteByte(56);
				ProtocolParser.WriteUInt32(stream, instance.RelegationRole);
			}
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator4 = instance.Attribute.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Attribute current4 = enumerator4.get_Current();
						stream.WriteByte(66);
						ProtocolParser.WriteUInt32(stream, current4.GetSerializedSize());
						bnet.protocol.attribute.Attribute.Serialize(stream, current4);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += ProtocolParser.SizeOfUInt32(this.Id);
			uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.Name);
			num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			if (this.Privilege.get_Count() > 0)
			{
				using (List<string>.Enumerator enumerator = this.Privilege.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						num += 1u;
						uint byteCount2 = (uint)Encoding.get_UTF8().GetByteCount(current);
						num += ProtocolParser.SizeOfUInt32(byteCount2) + byteCount2;
					}
				}
			}
			if (this.AssignableRole.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator2 = this.AssignableRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			if (this.HasRequired)
			{
				num += 1u;
				num += 1u;
			}
			if (this.HasUnique)
			{
				num += 1u;
				num += 1u;
			}
			if (this.HasRelegationRole)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.RelegationRole);
			}
			if (this.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator3 = this.Attribute.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Attribute current3 = enumerator3.get_Current();
						num += 1u;
						uint serializedSize = current3.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			num += 2u;
			return num;
		}

		public void SetId(uint val)
		{
			this.Id = val;
		}

		public void SetName(string val)
		{
			this.Name = val;
		}

		public void AddPrivilege(string val)
		{
			this._Privilege.Add(val);
		}

		public void ClearPrivilege()
		{
			this._Privilege.Clear();
		}

		public void SetPrivilege(List<string> val)
		{
			this.Privilege = val;
		}

		public void AddAssignableRole(uint val)
		{
			this._AssignableRole.Add(val);
		}

		public void ClearAssignableRole()
		{
			this._AssignableRole.Clear();
		}

		public void SetAssignableRole(List<uint> val)
		{
			this.AssignableRole = val;
		}

		public void SetRequired(bool val)
		{
			this.Required = val;
		}

		public void SetUnique(bool val)
		{
			this.Unique = val;
		}

		public void SetRelegationRole(uint val)
		{
			this.RelegationRole = val;
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
			num ^= this.Id.GetHashCode();
			num ^= this.Name.GetHashCode();
			using (List<string>.Enumerator enumerator = this.Privilege.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<uint>.Enumerator enumerator2 = this.AssignableRole.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			if (this.HasRequired)
			{
				num ^= this.Required.GetHashCode();
			}
			if (this.HasUnique)
			{
				num ^= this.Unique.GetHashCode();
			}
			if (this.HasRelegationRole)
			{
				num ^= this.RelegationRole.GetHashCode();
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
			Role role = obj as Role;
			if (role == null)
			{
				return false;
			}
			if (!this.Id.Equals(role.Id))
			{
				return false;
			}
			if (!this.Name.Equals(role.Name))
			{
				return false;
			}
			if (this.Privilege.get_Count() != role.Privilege.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Privilege.get_Count(); i++)
			{
				if (!this.Privilege.get_Item(i).Equals(role.Privilege.get_Item(i)))
				{
					return false;
				}
			}
			if (this.AssignableRole.get_Count() != role.AssignableRole.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.AssignableRole.get_Count(); j++)
			{
				if (!this.AssignableRole.get_Item(j).Equals(role.AssignableRole.get_Item(j)))
				{
					return false;
				}
			}
			if (this.HasRequired != role.HasRequired || (this.HasRequired && !this.Required.Equals(role.Required)))
			{
				return false;
			}
			if (this.HasUnique != role.HasUnique || (this.HasUnique && !this.Unique.Equals(role.Unique)))
			{
				return false;
			}
			if (this.HasRelegationRole != role.HasRelegationRole || (this.HasRelegationRole && !this.RelegationRole.Equals(role.RelegationRole)))
			{
				return false;
			}
			if (this.Attribute.get_Count() != role.Attribute.get_Count())
			{
				return false;
			}
			for (int k = 0; k < this.Attribute.get_Count(); k++)
			{
				if (!this.Attribute.get_Item(k).Equals(role.Attribute.get_Item(k)))
				{
					return false;
				}
			}
			return true;
		}

		public static Role ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<Role>(bs, 0, -1);
		}
	}
}
