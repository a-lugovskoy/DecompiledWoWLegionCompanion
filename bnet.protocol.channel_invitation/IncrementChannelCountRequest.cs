using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel_invitation
{
	public class IncrementChannelCountRequest : IProtoBuf
	{
		private List<ChannelCountDescription> _Descriptions = new List<ChannelCountDescription>();

		public EntityId AgentId
		{
			get;
			set;
		}

		public List<ChannelCountDescription> Descriptions
		{
			get
			{
				return this._Descriptions;
			}
			set
			{
				this._Descriptions = value;
			}
		}

		public List<ChannelCountDescription> DescriptionsList
		{
			get
			{
				return this._Descriptions;
			}
		}

		public int DescriptionsCount
		{
			get
			{
				return this._Descriptions.get_Count();
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
			IncrementChannelCountRequest.Deserialize(stream, this);
		}

		public static IncrementChannelCountRequest Deserialize(Stream stream, IncrementChannelCountRequest instance)
		{
			return IncrementChannelCountRequest.Deserialize(stream, instance, -1L);
		}

		public static IncrementChannelCountRequest DeserializeLengthDelimited(Stream stream)
		{
			IncrementChannelCountRequest incrementChannelCountRequest = new IncrementChannelCountRequest();
			IncrementChannelCountRequest.DeserializeLengthDelimited(stream, incrementChannelCountRequest);
			return incrementChannelCountRequest;
		}

		public static IncrementChannelCountRequest DeserializeLengthDelimited(Stream stream, IncrementChannelCountRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return IncrementChannelCountRequest.Deserialize(stream, instance, num);
		}

		public static IncrementChannelCountRequest Deserialize(Stream stream, IncrementChannelCountRequest instance, long limit)
		{
			if (instance.Descriptions == null)
			{
				instance.Descriptions = new List<ChannelCountDescription>();
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
						else
						{
							instance.Descriptions.Add(ChannelCountDescription.DeserializeLengthDelimited(stream));
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
			IncrementChannelCountRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, IncrementChannelCountRequest instance)
		{
			if (instance.AgentId == null)
			{
				throw new ArgumentNullException("AgentId", "Required by proto specification.");
			}
			stream.WriteByte(10);
			ProtocolParser.WriteUInt32(stream, instance.AgentId.GetSerializedSize());
			EntityId.Serialize(stream, instance.AgentId);
			if (instance.Descriptions.get_Count() > 0)
			{
				using (List<ChannelCountDescription>.Enumerator enumerator = instance.Descriptions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChannelCountDescription current = enumerator.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						ChannelCountDescription.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			uint serializedSize = this.AgentId.GetSerializedSize();
			num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			if (this.Descriptions.get_Count() > 0)
			{
				using (List<ChannelCountDescription>.Enumerator enumerator = this.Descriptions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChannelCountDescription current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
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

		public void AddDescriptions(ChannelCountDescription val)
		{
			this._Descriptions.Add(val);
		}

		public void ClearDescriptions()
		{
			this._Descriptions.Clear();
		}

		public void SetDescriptions(List<ChannelCountDescription> val)
		{
			this.Descriptions = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.AgentId.GetHashCode();
			using (List<ChannelCountDescription>.Enumerator enumerator = this.Descriptions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChannelCountDescription current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			IncrementChannelCountRequest incrementChannelCountRequest = obj as IncrementChannelCountRequest;
			if (incrementChannelCountRequest == null)
			{
				return false;
			}
			if (!this.AgentId.Equals(incrementChannelCountRequest.AgentId))
			{
				return false;
			}
			if (this.Descriptions.get_Count() != incrementChannelCountRequest.Descriptions.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Descriptions.get_Count(); i++)
			{
				if (!this.Descriptions.get_Item(i).Equals(incrementChannelCountRequest.Descriptions.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static IncrementChannelCountRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<IncrementChannelCountRequest>(bs, 0, -1);
		}
	}
}
