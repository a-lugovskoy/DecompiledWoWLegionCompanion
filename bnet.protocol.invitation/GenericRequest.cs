using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bnet.protocol.invitation
{
	public class GenericRequest : IProtoBuf
	{
		public bool HasAgentId;

		private EntityId _AgentId;

		public bool HasTargetId;

		private EntityId _TargetId;

		public bool HasInviteeName;

		private string _InviteeName;

		public bool HasInviterName;

		private string _InviterName;

		private List<uint> _PreviousRole = new List<uint>();

		private List<uint> _DesiredRole = new List<uint>();

		public bool HasReason;

		private uint _Reason;

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
			get
			{
				return this._TargetId;
			}
			set
			{
				this._TargetId = value;
				this.HasTargetId = (value != null);
			}
		}

		public ulong InvitationId
		{
			get;
			set;
		}

		public string InviteeName
		{
			get
			{
				return this._InviteeName;
			}
			set
			{
				this._InviteeName = value;
				this.HasInviteeName = (value != null);
			}
		}

		public string InviterName
		{
			get
			{
				return this._InviterName;
			}
			set
			{
				this._InviterName = value;
				this.HasInviterName = (value != null);
			}
		}

		public List<uint> PreviousRole
		{
			get
			{
				return this._PreviousRole;
			}
			set
			{
				this._PreviousRole = value;
			}
		}

		public List<uint> PreviousRoleList
		{
			get
			{
				return this._PreviousRole;
			}
		}

		public int PreviousRoleCount
		{
			get
			{
				return this._PreviousRole.get_Count();
			}
		}

		public List<uint> DesiredRole
		{
			get
			{
				return this._DesiredRole;
			}
			set
			{
				this._DesiredRole = value;
			}
		}

		public List<uint> DesiredRoleList
		{
			get
			{
				return this._DesiredRole;
			}
		}

		public int DesiredRoleCount
		{
			get
			{
				return this._DesiredRole.get_Count();
			}
		}

		public uint Reason
		{
			get
			{
				return this._Reason;
			}
			set
			{
				this._Reason = value;
				this.HasReason = true;
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
			GenericRequest.Deserialize(stream, this);
		}

		public static GenericRequest Deserialize(Stream stream, GenericRequest instance)
		{
			return GenericRequest.Deserialize(stream, instance, -1L);
		}

		public static GenericRequest DeserializeLengthDelimited(Stream stream)
		{
			GenericRequest genericRequest = new GenericRequest();
			GenericRequest.DeserializeLengthDelimited(stream, genericRequest);
			return genericRequest;
		}

		public static GenericRequest DeserializeLengthDelimited(Stream stream, GenericRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GenericRequest.Deserialize(stream, instance, num);
		}

		public static GenericRequest Deserialize(Stream stream, GenericRequest instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.PreviousRole == null)
			{
				instance.PreviousRole = new List<uint>();
			}
			if (instance.DesiredRole == null)
			{
				instance.DesiredRole = new List<uint>();
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
							if (num2 != 25)
							{
								if (num2 != 34)
								{
									if (num2 != 42)
									{
										if (num2 != 50)
										{
											if (num2 != 58)
											{
												if (num2 != 64)
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
													instance.Reason = ProtocolParser.ReadUInt32(stream);
												}
											}
											else
											{
												long num3 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
												num3 += stream.get_Position();
												while (stream.get_Position() < num3)
												{
													instance.DesiredRole.Add(ProtocolParser.ReadUInt32(stream));
												}
												if (stream.get_Position() != num3)
												{
													throw new ProtocolBufferException("Read too many bytes in packed data");
												}
											}
										}
										else
										{
											long num4 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
											num4 += stream.get_Position();
											while (stream.get_Position() < num4)
											{
												instance.PreviousRole.Add(ProtocolParser.ReadUInt32(stream));
											}
											if (stream.get_Position() != num4)
											{
												throw new ProtocolBufferException("Read too many bytes in packed data");
											}
										}
									}
									else
									{
										instance.InviterName = ProtocolParser.ReadString(stream);
									}
								}
								else
								{
									instance.InviteeName = ProtocolParser.ReadString(stream);
								}
							}
							else
							{
								instance.InvitationId = binaryReader.ReadUInt64();
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
			GenericRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GenericRequest instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.HasAgentId)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.AgentId.GetSerializedSize());
				EntityId.Serialize(stream, instance.AgentId);
			}
			if (instance.HasTargetId)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteUInt32(stream, instance.TargetId.GetSerializedSize());
				EntityId.Serialize(stream, instance.TargetId);
			}
			stream.WriteByte(25);
			binaryWriter.Write(instance.InvitationId);
			if (instance.HasInviteeName)
			{
				stream.WriteByte(34);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.InviteeName));
			}
			if (instance.HasInviterName)
			{
				stream.WriteByte(42);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.InviterName));
			}
			if (instance.PreviousRole.get_Count() > 0)
			{
				stream.WriteByte(50);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator = instance.PreviousRole.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator2 = instance.PreviousRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						ProtocolParser.WriteUInt32(stream, current2);
					}
				}
			}
			if (instance.DesiredRole.get_Count() > 0)
			{
				stream.WriteByte(58);
				uint num2 = 0u;
				using (List<uint>.Enumerator enumerator3 = instance.DesiredRole.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						uint current3 = enumerator3.get_Current();
						num2 += ProtocolParser.SizeOfUInt32(current3);
					}
				}
				ProtocolParser.WriteUInt32(stream, num2);
				using (List<uint>.Enumerator enumerator4 = instance.DesiredRole.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						uint current4 = enumerator4.get_Current();
						ProtocolParser.WriteUInt32(stream, current4);
					}
				}
			}
			if (instance.HasReason)
			{
				stream.WriteByte(64);
				ProtocolParser.WriteUInt32(stream, instance.Reason);
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
			if (this.HasTargetId)
			{
				num += 1u;
				uint serializedSize2 = this.TargetId.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
			num += 8u;
			if (this.HasInviteeName)
			{
				num += 1u;
				uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.InviteeName);
				num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			}
			if (this.HasInviterName)
			{
				num += 1u;
				uint byteCount2 = (uint)Encoding.get_UTF8().GetByteCount(this.InviterName);
				num += ProtocolParser.SizeOfUInt32(byteCount2) + byteCount2;
			}
			if (this.PreviousRole.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator = this.PreviousRole.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			if (this.DesiredRole.get_Count() > 0)
			{
				num += 1u;
				uint num3 = num;
				using (List<uint>.Enumerator enumerator2 = this.DesiredRole.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						num += ProtocolParser.SizeOfUInt32(current2);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num3);
			}
			if (this.HasReason)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.Reason);
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

		public void SetInvitationId(ulong val)
		{
			this.InvitationId = val;
		}

		public void SetInviteeName(string val)
		{
			this.InviteeName = val;
		}

		public void SetInviterName(string val)
		{
			this.InviterName = val;
		}

		public void AddPreviousRole(uint val)
		{
			this._PreviousRole.Add(val);
		}

		public void ClearPreviousRole()
		{
			this._PreviousRole.Clear();
		}

		public void SetPreviousRole(List<uint> val)
		{
			this.PreviousRole = val;
		}

		public void AddDesiredRole(uint val)
		{
			this._DesiredRole.Add(val);
		}

		public void ClearDesiredRole()
		{
			this._DesiredRole.Clear();
		}

		public void SetDesiredRole(List<uint> val)
		{
			this.DesiredRole = val;
		}

		public void SetReason(uint val)
		{
			this.Reason = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasAgentId)
			{
				num ^= this.AgentId.GetHashCode();
			}
			if (this.HasTargetId)
			{
				num ^= this.TargetId.GetHashCode();
			}
			num ^= this.InvitationId.GetHashCode();
			if (this.HasInviteeName)
			{
				num ^= this.InviteeName.GetHashCode();
			}
			if (this.HasInviterName)
			{
				num ^= this.InviterName.GetHashCode();
			}
			using (List<uint>.Enumerator enumerator = this.PreviousRole.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<uint>.Enumerator enumerator2 = this.DesiredRole.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			if (this.HasReason)
			{
				num ^= this.Reason.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GenericRequest genericRequest = obj as GenericRequest;
			if (genericRequest == null)
			{
				return false;
			}
			if (this.HasAgentId != genericRequest.HasAgentId || (this.HasAgentId && !this.AgentId.Equals(genericRequest.AgentId)))
			{
				return false;
			}
			if (this.HasTargetId != genericRequest.HasTargetId || (this.HasTargetId && !this.TargetId.Equals(genericRequest.TargetId)))
			{
				return false;
			}
			if (!this.InvitationId.Equals(genericRequest.InvitationId))
			{
				return false;
			}
			if (this.HasInviteeName != genericRequest.HasInviteeName || (this.HasInviteeName && !this.InviteeName.Equals(genericRequest.InviteeName)))
			{
				return false;
			}
			if (this.HasInviterName != genericRequest.HasInviterName || (this.HasInviterName && !this.InviterName.Equals(genericRequest.InviterName)))
			{
				return false;
			}
			if (this.PreviousRole.get_Count() != genericRequest.PreviousRole.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.PreviousRole.get_Count(); i++)
			{
				if (!this.PreviousRole.get_Item(i).Equals(genericRequest.PreviousRole.get_Item(i)))
				{
					return false;
				}
			}
			if (this.DesiredRole.get_Count() != genericRequest.DesiredRole.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.DesiredRole.get_Count(); j++)
			{
				if (!this.DesiredRole.get_Item(j).Equals(genericRequest.DesiredRole.get_Item(j)))
				{
					return false;
				}
			}
			return this.HasReason == genericRequest.HasReason && (!this.HasReason || this.Reason.Equals(genericRequest.Reason));
		}

		public static GenericRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GenericRequest>(bs, 0, -1);
		}
	}
}
