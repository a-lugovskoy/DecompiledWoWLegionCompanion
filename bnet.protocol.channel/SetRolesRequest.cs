using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel
{
	public class SetRolesRequest : IProtoBuf
	{
		public bool HasAgentId;

		private EntityId _AgentId;

		private List<uint> _Role = new List<uint>();

		private List<EntityId> _MemberId = new List<EntityId>();

		public EntityId AgentId
		{
			get
			{
				return this._AgentId;
			}
			set
			{
				this._AgentId = value;
				this.HasAgentId = (value != null);
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

		public List<EntityId> MemberId
		{
			get
			{
				return this._MemberId;
			}
			set
			{
				this._MemberId = value;
			}
		}

		public List<EntityId> MemberIdList
		{
			get
			{
				return this._MemberId;
			}
		}

		public int MemberIdCount
		{
			get
			{
				return this._MemberId.get_Count();
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
			SetRolesRequest.Deserialize(stream, this);
		}

		public static SetRolesRequest Deserialize(Stream stream, SetRolesRequest instance)
		{
			return SetRolesRequest.Deserialize(stream, instance, -1L);
		}

		public static SetRolesRequest DeserializeLengthDelimited(Stream stream)
		{
			SetRolesRequest setRolesRequest = new SetRolesRequest();
			SetRolesRequest.DeserializeLengthDelimited(stream, setRolesRequest);
			return setRolesRequest;
		}

		public static SetRolesRequest DeserializeLengthDelimited(Stream stream, SetRolesRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return SetRolesRequest.Deserialize(stream, instance, num);
		}

		public static SetRolesRequest Deserialize(Stream stream, SetRolesRequest instance, long limit)
		{
			if (instance.Role == null)
			{
				instance.Role = new List<uint>();
			}
			if (instance.MemberId == null)
			{
				instance.MemberId = new List<EntityId>();
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
								instance.MemberId.Add(EntityId.DeserializeLengthDelimited(stream));
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
					else if (instance.AgentId == null)
					{
						instance.AgentId = EntityId.DeserializeLengthDelimited(stream);
					}
					else
					{
						EntityId.DeserializeLengthDelimited(stream, instance.AgentId);
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
			SetRolesRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, SetRolesRequest instance)
		{
			if (instance.HasAgentId)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.AgentId.GetSerializedSize());
				EntityId.Serialize(stream, instance.AgentId);
			}
			if (instance.Role.get_Count() > 0)
			{
				stream.WriteByte(18);
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
			if (instance.MemberId.get_Count() > 0)
			{
				using (List<EntityId>.Enumerator enumerator3 = instance.MemberId.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						EntityId current3 = enumerator3.get_Current();
						stream.WriteByte(26);
						ProtocolParser.WriteUInt32(stream, current3.GetSerializedSize());
						EntityId.Serialize(stream, current3);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasAgentId)
			{
				num += 1u;
				uint serializedSize = this.AgentId.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
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
			if (this.MemberId.get_Count() > 0)
			{
				using (List<EntityId>.Enumerator enumerator2 = this.MemberId.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						EntityId current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize2 = current2.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			return num;
		}

		public void SetAgentId(EntityId val)
		{
			this.AgentId = val;
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

		public void AddMemberId(EntityId val)
		{
			this._MemberId.Add(val);
		}

		public void ClearMemberId()
		{
			this._MemberId.Clear();
		}

		public void SetMemberId(List<EntityId> val)
		{
			this.MemberId = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasAgentId)
			{
				num ^= this.AgentId.GetHashCode();
			}
			using (List<uint>.Enumerator enumerator = this.Role.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<EntityId>.Enumerator enumerator2 = this.MemberId.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					EntityId current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			SetRolesRequest setRolesRequest = obj as SetRolesRequest;
			if (setRolesRequest == null)
			{
				return false;
			}
			if (this.HasAgentId != setRolesRequest.HasAgentId || (this.HasAgentId && !this.AgentId.Equals(setRolesRequest.AgentId)))
			{
				return false;
			}
			if (this.Role.get_Count() != setRolesRequest.Role.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Role.get_Count(); i++)
			{
				if (!this.Role.get_Item(i).Equals(setRolesRequest.Role.get_Item(i)))
				{
					return false;
				}
			}
			if (this.MemberId.get_Count() != setRolesRequest.MemberId.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.MemberId.get_Count(); j++)
			{
				if (!this.MemberId.get_Item(j).Equals(setRolesRequest.MemberId.get_Item(j)))
				{
					return false;
				}
			}
			return true;
		}

		public static SetRolesRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<SetRolesRequest>(bs, 0, -1);
		}
	}
}
