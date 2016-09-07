using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.friends
{
	public class UpdateFriendStateRequest : IProtoBuf
	{
		public bool HasAgentId;

		private EntityId _AgentId;

		private List<Attribute> _Attribute = new List<Attribute>();

		public bool HasAttributesEpoch;

		private ulong _AttributesEpoch;

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

		public ulong AttributesEpoch
		{
			get
			{
				return this._AttributesEpoch;
			}
			set
			{
				this._AttributesEpoch = value;
				this.HasAttributesEpoch = true;
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
			UpdateFriendStateRequest.Deserialize(stream, this);
		}

		public static UpdateFriendStateRequest Deserialize(Stream stream, UpdateFriendStateRequest instance)
		{
			return UpdateFriendStateRequest.Deserialize(stream, instance, -1L);
		}

		public static UpdateFriendStateRequest DeserializeLengthDelimited(Stream stream)
		{
			UpdateFriendStateRequest updateFriendStateRequest = new UpdateFriendStateRequest();
			UpdateFriendStateRequest.DeserializeLengthDelimited(stream, updateFriendStateRequest);
			return updateFriendStateRequest;
		}

		public static UpdateFriendStateRequest DeserializeLengthDelimited(Stream stream, UpdateFriendStateRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return UpdateFriendStateRequest.Deserialize(stream, instance, num);
		}

		public static UpdateFriendStateRequest Deserialize(Stream stream, UpdateFriendStateRequest instance, long limit)
		{
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
								if (num2 != 32)
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
									instance.AttributesEpoch = ProtocolParser.ReadUInt64(stream);
								}
							}
							else
							{
								instance.Attribute.Add(bnet.protocol.attribute.Attribute.DeserializeLengthDelimited(stream));
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
			UpdateFriendStateRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, UpdateFriendStateRequest instance)
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
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = instance.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						stream.WriteByte(26);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.attribute.Attribute.Serialize(stream, current);
					}
				}
			}
			if (instance.HasAttributesEpoch)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteUInt64(stream, instance.AttributesEpoch);
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
			if (this.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						num += 1u;
						uint serializedSize3 = current.GetSerializedSize();
						num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
					}
				}
			}
			if (this.HasAttributesEpoch)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64(this.AttributesEpoch);
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

		public void SetAttributesEpoch(ulong val)
		{
			this.AttributesEpoch = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasAgentId)
			{
				num ^= this.AgentId.GetHashCode();
			}
			num ^= this.TargetId.GetHashCode();
			using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			if (this.HasAttributesEpoch)
			{
				num ^= this.AttributesEpoch.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			UpdateFriendStateRequest updateFriendStateRequest = obj as UpdateFriendStateRequest;
			if (updateFriendStateRequest == null)
			{
				return false;
			}
			if (this.HasAgentId != updateFriendStateRequest.HasAgentId || (this.HasAgentId && !this.AgentId.Equals(updateFriendStateRequest.AgentId)))
			{
				return false;
			}
			if (!this.TargetId.Equals(updateFriendStateRequest.TargetId))
			{
				return false;
			}
			if (this.Attribute.get_Count() != updateFriendStateRequest.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(updateFriendStateRequest.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasAttributesEpoch == updateFriendStateRequest.HasAttributesEpoch && (!this.HasAttributesEpoch || this.AttributesEpoch.Equals(updateFriendStateRequest.AttributesEpoch));
		}

		public static UpdateFriendStateRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<UpdateFriendStateRequest>(bs, 0, -1);
		}
	}
}
