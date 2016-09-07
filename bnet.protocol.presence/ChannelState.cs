using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.presence
{
	public class ChannelState : IProtoBuf
	{
		public bool HasEntityId;

		private EntityId _EntityId;

		private List<FieldOperation> _FieldOperation = new List<FieldOperation>();

		public bool HasHealing;

		private bool _Healing;

		public EntityId EntityId
		{
			get
			{
				return this._EntityId;
			}
			set
			{
				this._EntityId = value;
				this.HasEntityId = (value != null);
			}
		}

		public List<FieldOperation> FieldOperation
		{
			get
			{
				return this._FieldOperation;
			}
			set
			{
				this._FieldOperation = value;
			}
		}

		public List<FieldOperation> FieldOperationList
		{
			get
			{
				return this._FieldOperation;
			}
		}

		public int FieldOperationCount
		{
			get
			{
				return this._FieldOperation.get_Count();
			}
		}

		public bool Healing
		{
			get
			{
				return this._Healing;
			}
			set
			{
				this._Healing = value;
				this.HasHealing = true;
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
			ChannelState.Deserialize(stream, this);
		}

		public static ChannelState Deserialize(Stream stream, ChannelState instance)
		{
			return ChannelState.Deserialize(stream, instance, -1L);
		}

		public static ChannelState DeserializeLengthDelimited(Stream stream)
		{
			ChannelState channelState = new ChannelState();
			ChannelState.DeserializeLengthDelimited(stream, channelState);
			return channelState;
		}

		public static ChannelState DeserializeLengthDelimited(Stream stream, ChannelState instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ChannelState.Deserialize(stream, instance, num);
		}

		public static ChannelState Deserialize(Stream stream, ChannelState instance, long limit)
		{
			if (instance.FieldOperation == null)
			{
				instance.FieldOperation = new List<FieldOperation>();
			}
			instance.Healing = false;
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
								instance.Healing = ProtocolParser.ReadBool(stream);
							}
						}
						else
						{
							instance.FieldOperation.Add(bnet.protocol.presence.FieldOperation.DeserializeLengthDelimited(stream));
						}
					}
					else if (instance.EntityId == null)
					{
						instance.EntityId = EntityId.DeserializeLengthDelimited(stream);
					}
					else
					{
						EntityId.DeserializeLengthDelimited(stream, instance.EntityId);
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
			ChannelState.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ChannelState instance)
		{
			if (instance.HasEntityId)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.EntityId.GetSerializedSize());
				EntityId.Serialize(stream, instance.EntityId);
			}
			if (instance.FieldOperation.get_Count() > 0)
			{
				using (List<FieldOperation>.Enumerator enumerator = instance.FieldOperation.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FieldOperation current = enumerator.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.presence.FieldOperation.Serialize(stream, current);
					}
				}
			}
			if (instance.HasHealing)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteBool(stream, instance.Healing);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasEntityId)
			{
				num += 1u;
				uint serializedSize = this.EntityId.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.FieldOperation.get_Count() > 0)
			{
				using (List<FieldOperation>.Enumerator enumerator = this.FieldOperation.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FieldOperation current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			if (this.HasHealing)
			{
				num += 1u;
				num += 1u;
			}
			return num;
		}

		public void SetEntityId(EntityId val)
		{
			this.EntityId = val;
		}

		public void AddFieldOperation(FieldOperation val)
		{
			this._FieldOperation.Add(val);
		}

		public void ClearFieldOperation()
		{
			this._FieldOperation.Clear();
		}

		public void SetFieldOperation(List<FieldOperation> val)
		{
			this.FieldOperation = val;
		}

		public void SetHealing(bool val)
		{
			this.Healing = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasEntityId)
			{
				num ^= this.EntityId.GetHashCode();
			}
			using (List<FieldOperation>.Enumerator enumerator = this.FieldOperation.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FieldOperation current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			if (this.HasHealing)
			{
				num ^= this.Healing.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ChannelState channelState = obj as ChannelState;
			if (channelState == null)
			{
				return false;
			}
			if (this.HasEntityId != channelState.HasEntityId || (this.HasEntityId && !this.EntityId.Equals(channelState.EntityId)))
			{
				return false;
			}
			if (this.FieldOperation.get_Count() != channelState.FieldOperation.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.FieldOperation.get_Count(); i++)
			{
				if (!this.FieldOperation.get_Item(i).Equals(channelState.FieldOperation.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasHealing == channelState.HasHealing && (!this.HasHealing || this.Healing.Equals(channelState.Healing));
		}

		public static ChannelState ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ChannelState>(bs, 0, -1);
		}
	}
}
