using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel
{
	public class UpdateMemberStateRequest : IProtoBuf
	{
		public bool HasAgentId;

		private EntityId _AgentId;

		private List<Member> _StateChange = new List<Member>();

		private List<uint> _RemovedRole = new List<uint>();

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

		public List<Member> StateChange
		{
			get
			{
				return this._StateChange;
			}
			set
			{
				this._StateChange = value;
			}
		}

		public List<Member> StateChangeList
		{
			get
			{
				return this._StateChange;
			}
		}

		public int StateChangeCount
		{
			get
			{
				return this._StateChange.get_Count();
			}
		}

		public List<uint> RemovedRole
		{
			get
			{
				return this._RemovedRole;
			}
			set
			{
				this._RemovedRole = value;
			}
		}

		public List<uint> RemovedRoleList
		{
			get
			{
				return this._RemovedRole;
			}
		}

		public int RemovedRoleCount
		{
			get
			{
				return this._RemovedRole.get_Count();
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
			UpdateMemberStateRequest.Deserialize(stream, this);
		}

		public static UpdateMemberStateRequest Deserialize(Stream stream, UpdateMemberStateRequest instance)
		{
			return UpdateMemberStateRequest.Deserialize(stream, instance, -1L);
		}

		public static UpdateMemberStateRequest DeserializeLengthDelimited(Stream stream)
		{
			UpdateMemberStateRequest updateMemberStateRequest = new UpdateMemberStateRequest();
			UpdateMemberStateRequest.DeserializeLengthDelimited(stream, updateMemberStateRequest);
			return updateMemberStateRequest;
		}

		public static UpdateMemberStateRequest DeserializeLengthDelimited(Stream stream, UpdateMemberStateRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return UpdateMemberStateRequest.Deserialize(stream, instance, num);
		}

		public static UpdateMemberStateRequest Deserialize(Stream stream, UpdateMemberStateRequest instance, long limit)
		{
			if (instance.StateChange == null)
			{
				instance.StateChange = new List<Member>();
			}
			if (instance.RemovedRole == null)
			{
				instance.RemovedRole = new List<uint>();
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
								long num3 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
								num3 += stream.get_Position();
								while (stream.get_Position() < num3)
								{
									instance.RemovedRole.Add(ProtocolParser.ReadUInt32(stream));
								}
								if (stream.get_Position() != num3)
								{
									throw new ProtocolBufferException("Read too many bytes in packed data");
								}
							}
						}
						else
						{
							instance.StateChange.Add(Member.DeserializeLengthDelimited(stream));
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
			UpdateMemberStateRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, UpdateMemberStateRequest instance)
		{
			if (instance.HasAgentId)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.AgentId.GetSerializedSize());
				EntityId.Serialize(stream, instance.AgentId);
			}
			if (instance.StateChange.get_Count() > 0)
			{
				using (List<Member>.Enumerator enumerator = instance.StateChange.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Member current = enumerator.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						Member.Serialize(stream, current);
					}
				}
			}
			if (instance.RemovedRole.get_Count() > 0)
			{
				stream.WriteByte(26);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator2 = instance.RemovedRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator3 = instance.RemovedRole.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						uint current3 = enumerator3.get_Current();
						ProtocolParser.WriteUInt32(stream, current3);
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
			if (this.StateChange.get_Count() > 0)
			{
				using (List<Member>.Enumerator enumerator = this.StateChange.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Member current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			if (this.RemovedRole.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator2 = this.RemovedRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			return num;
		}

		public void SetAgentId(EntityId val)
		{
			this.AgentId = val;
		}

		public void AddStateChange(Member val)
		{
			this._StateChange.Add(val);
		}

		public void ClearStateChange()
		{
			this._StateChange.Clear();
		}

		public void SetStateChange(List<Member> val)
		{
			this.StateChange = val;
		}

		public void AddRemovedRole(uint val)
		{
			this._RemovedRole.Add(val);
		}

		public void ClearRemovedRole()
		{
			this._RemovedRole.Clear();
		}

		public void SetRemovedRole(List<uint> val)
		{
			this.RemovedRole = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasAgentId)
			{
				num ^= this.AgentId.GetHashCode();
			}
			using (List<Member>.Enumerator enumerator = this.StateChange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Member current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<uint>.Enumerator enumerator2 = this.RemovedRole.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			UpdateMemberStateRequest updateMemberStateRequest = obj as UpdateMemberStateRequest;
			if (updateMemberStateRequest == null)
			{
				return false;
			}
			if (this.HasAgentId != updateMemberStateRequest.HasAgentId || (this.HasAgentId && !this.AgentId.Equals(updateMemberStateRequest.AgentId)))
			{
				return false;
			}
			if (this.StateChange.get_Count() != updateMemberStateRequest.StateChange.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.StateChange.get_Count(); i++)
			{
				if (!this.StateChange.get_Item(i).Equals(updateMemberStateRequest.StateChange.get_Item(i)))
				{
					return false;
				}
			}
			if (this.RemovedRole.get_Count() != updateMemberStateRequest.RemovedRole.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.RemovedRole.get_Count(); j++)
			{
				if (!this.RemovedRole.get_Item(j).Equals(updateMemberStateRequest.RemovedRole.get_Item(j)))
				{
					return false;
				}
			}
			return true;
		}

		public static UpdateMemberStateRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<UpdateMemberStateRequest>(bs, 0, -1);
		}
	}
}
