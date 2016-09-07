using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol
{
	public class RoleSetConfig : IProtoBuf
	{
		private List<Privilege> _Privilege = new List<Privilege>();

		public List<Privilege> Privilege
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

		public List<Privilege> PrivilegeList
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

		public RoleSet RoleSet
		{
			get;
			set;
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
			RoleSetConfig.Deserialize(stream, this);
		}

		public static RoleSetConfig Deserialize(Stream stream, RoleSetConfig instance)
		{
			return RoleSetConfig.Deserialize(stream, instance, -1L);
		}

		public static RoleSetConfig DeserializeLengthDelimited(Stream stream)
		{
			RoleSetConfig roleSetConfig = new RoleSetConfig();
			RoleSetConfig.DeserializeLengthDelimited(stream, roleSetConfig);
			return roleSetConfig;
		}

		public static RoleSetConfig DeserializeLengthDelimited(Stream stream, RoleSetConfig instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return RoleSetConfig.Deserialize(stream, instance, num);
		}

		public static RoleSetConfig Deserialize(Stream stream, RoleSetConfig instance, long limit)
		{
			if (instance.Privilege == null)
			{
				instance.Privilege = new List<Privilege>();
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
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							uint field = key.Field;
							if (field == 0u)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						else if (instance.RoleSet == null)
						{
							instance.RoleSet = RoleSet.DeserializeLengthDelimited(stream);
						}
						else
						{
							RoleSet.DeserializeLengthDelimited(stream, instance.RoleSet);
						}
					}
					else
					{
						instance.Privilege.Add(bnet.protocol.Privilege.DeserializeLengthDelimited(stream));
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
			RoleSetConfig.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, RoleSetConfig instance)
		{
			if (instance.Privilege.get_Count() > 0)
			{
				using (List<Privilege>.Enumerator enumerator = instance.Privilege.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Privilege current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.Privilege.Serialize(stream, current);
					}
				}
			}
			if (instance.RoleSet == null)
			{
				throw new ArgumentNullException("RoleSet", "Required by proto specification.");
			}
			stream.WriteByte(18);
			ProtocolParser.WriteUInt32(stream, instance.RoleSet.GetSerializedSize());
			RoleSet.Serialize(stream, instance.RoleSet);
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Privilege.get_Count() > 0)
			{
				using (List<Privilege>.Enumerator enumerator = this.Privilege.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Privilege current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			uint serializedSize2 = this.RoleSet.GetSerializedSize();
			num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			num += 1u;
			return num;
		}

		public void AddPrivilege(Privilege val)
		{
			this._Privilege.Add(val);
		}

		public void ClearPrivilege()
		{
			this._Privilege.Clear();
		}

		public void SetPrivilege(List<Privilege> val)
		{
			this.Privilege = val;
		}

		public void SetRoleSet(RoleSet val)
		{
			this.RoleSet = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<Privilege>.Enumerator enumerator = this.Privilege.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Privilege current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			num ^= this.RoleSet.GetHashCode();
			return num;
		}

		public override bool Equals(object obj)
		{
			RoleSetConfig roleSetConfig = obj as RoleSetConfig;
			if (roleSetConfig == null)
			{
				return false;
			}
			if (this.Privilege.get_Count() != roleSetConfig.Privilege.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Privilege.get_Count(); i++)
			{
				if (!this.Privilege.get_Item(i).Equals(roleSetConfig.Privilege.get_Item(i)))
				{
					return false;
				}
			}
			return this.RoleSet.Equals(roleSetConfig.RoleSet);
		}

		public static RoleSetConfig ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<RoleSetConfig>(bs, 0, -1);
		}
	}
}
