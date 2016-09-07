using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.friends
{
	public class AssignRoleRequest : IProtoBuf
	{
		public bool HasAgentId;

		private EntityId _AgentId;

		private List<int> _Role = new List<int>();

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

		public EntityId TargetId
		{
			get;
			set;
		}

		public List<int> Role
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

		public List<int> RoleList
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
			AssignRoleRequest.Deserialize(stream, this);
		}

		public static AssignRoleRequest Deserialize(Stream stream, AssignRoleRequest instance)
		{
			return AssignRoleRequest.Deserialize(stream, instance, -1L);
		}

		public static AssignRoleRequest DeserializeLengthDelimited(Stream stream)
		{
			AssignRoleRequest assignRoleRequest = new AssignRoleRequest();
			AssignRoleRequest.DeserializeLengthDelimited(stream, assignRoleRequest);
			return assignRoleRequest;
		}

		public static AssignRoleRequest DeserializeLengthDelimited(Stream stream, AssignRoleRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return AssignRoleRequest.Deserialize(stream, instance, num);
		}

		public static AssignRoleRequest Deserialize(Stream stream, AssignRoleRequest instance, long limit)
		{
			if (instance.Role == null)
			{
				instance.Role = new List<int>();
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
							if (num2 != 24)
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
								instance.Role.Add((int)ProtocolParser.ReadUInt64(stream));
							}
						}
						else if (instance.TargetId == null)
						{
							instance.TargetId = EntityId.DeserializeLengthDelimited(stream);
						}
						else
						{
							EntityId.DeserializeLengthDelimited(stream, instance.TargetId);
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
			AssignRoleRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, AssignRoleRequest instance)
		{
			if (instance.HasAgentId)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.AgentId.GetSerializedSize());
				EntityId.Serialize(stream, instance.AgentId);
			}
			if (instance.TargetId == null)
			{
				throw new ArgumentNullException("TargetId", "Required by proto specification.");
			}
			stream.WriteByte(18);
			ProtocolParser.WriteUInt32(stream, instance.TargetId.GetSerializedSize());
			EntityId.Serialize(stream, instance.TargetId);
			if (instance.Role.get_Count() > 0)
			{
				using (List<int>.Enumerator enumerator = instance.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						stream.WriteByte(24);
						ProtocolParser.WriteUInt64(stream, (ulong)((long)current));
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
			uint serializedSize2 = this.TargetId.GetSerializedSize();
			num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			if (this.Role.get_Count() > 0)
			{
				using (List<int>.Enumerator enumerator = this.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						num += 1u;
						num += ProtocolParser.SizeOfUInt64((ulong)((long)current));
					}
				}
			}
			num += 1u;
			return num;
		}

		public void SetAgentId(EntityId val)
		{
			this.AgentId = val;
		}

		public void SetTargetId(EntityId val)
		{
			this.TargetId = val;
		}

		public void AddRole(int val)
		{
			this._Role.Add(val);
		}

		public void ClearRole()
		{
			this._Role.Clear();
		}

		public void SetRole(List<int> val)
		{
			this.Role = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasAgentId)
			{
				num ^= this.AgentId.GetHashCode();
			}
			num ^= this.TargetId.GetHashCode();
			using (List<int>.Enumerator enumerator = this.Role.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			AssignRoleRequest assignRoleRequest = obj as AssignRoleRequest;
			if (assignRoleRequest == null)
			{
				return false;
			}
			if (this.HasAgentId != assignRoleRequest.HasAgentId || (this.HasAgentId && !this.AgentId.Equals(assignRoleRequest.AgentId)))
			{
				return false;
			}
			if (!this.TargetId.Equals(assignRoleRequest.TargetId))
			{
				return false;
			}
			if (this.Role.get_Count() != assignRoleRequest.Role.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Role.get_Count(); i++)
			{
				if (!this.Role.get_Item(i).Equals(assignRoleRequest.Role.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static AssignRoleRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<AssignRoleRequest>(bs, 0, -1);
		}
	}
}
