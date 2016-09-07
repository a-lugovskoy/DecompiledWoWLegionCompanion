using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.challenge
{
	public class SendChallengeToUserRequest : IProtoBuf
	{
		public bool HasPeerId;

		private ProcessId _PeerId;

		public bool HasGameAccountId;

		private EntityId _GameAccountId;

		private List<Challenge> _Challenges = new List<Challenge>();

		public bool HasTimeout;

		private ulong _Timeout;

		private List<Attribute> _Attributes = new List<Attribute>();

		public ProcessId PeerId
		{
			get
			{
				return this._PeerId;
			}
			set
			{
				this._PeerId = value;
				this.HasPeerId = (value != null);
			}
		}

		public EntityId GameAccountId
		{
			get
			{
				return this._GameAccountId;
			}
			set
			{
				this._GameAccountId = value;
				this.HasGameAccountId = (value != null);
			}
		}

		public List<Challenge> Challenges
		{
			get
			{
				return this._Challenges;
			}
			set
			{
				this._Challenges = value;
			}
		}

		public List<Challenge> ChallengesList
		{
			get
			{
				return this._Challenges;
			}
		}

		public int ChallengesCount
		{
			get
			{
				return this._Challenges.get_Count();
			}
		}

		public uint Context
		{
			get;
			set;
		}

		public ulong Timeout
		{
			get
			{
				return this._Timeout;
			}
			set
			{
				this._Timeout = value;
				this.HasTimeout = true;
			}
		}

		public List<Attribute> Attributes
		{
			get
			{
				return this._Attributes;
			}
			set
			{
				this._Attributes = value;
			}
		}

		public List<Attribute> AttributesList
		{
			get
			{
				return this._Attributes;
			}
		}

		public int AttributesCount
		{
			get
			{
				return this._Attributes.get_Count();
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
			SendChallengeToUserRequest.Deserialize(stream, this);
		}

		public static SendChallengeToUserRequest Deserialize(Stream stream, SendChallengeToUserRequest instance)
		{
			return SendChallengeToUserRequest.Deserialize(stream, instance, -1L);
		}

		public static SendChallengeToUserRequest DeserializeLengthDelimited(Stream stream)
		{
			SendChallengeToUserRequest sendChallengeToUserRequest = new SendChallengeToUserRequest();
			SendChallengeToUserRequest.DeserializeLengthDelimited(stream, sendChallengeToUserRequest);
			return sendChallengeToUserRequest;
		}

		public static SendChallengeToUserRequest DeserializeLengthDelimited(Stream stream, SendChallengeToUserRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return SendChallengeToUserRequest.Deserialize(stream, instance, num);
		}

		public static SendChallengeToUserRequest Deserialize(Stream stream, SendChallengeToUserRequest instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.Challenges == null)
			{
				instance.Challenges = new List<Challenge>();
			}
			if (instance.Attributes == null)
			{
				instance.Attributes = new List<Attribute>();
			}
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num != -1)
				{
					int num2 = num;
					switch (num2)
					{
					case 37:
						instance.Context = binaryReader.ReadUInt32();
						continue;
					case 38:
					case 39:
					{
						IL_9F:
						if (num2 == 10)
						{
							if (instance.PeerId == null)
							{
								instance.PeerId = ProcessId.DeserializeLengthDelimited(stream);
							}
							else
							{
								ProcessId.DeserializeLengthDelimited(stream, instance.PeerId);
							}
							continue;
						}
						if (num2 == 18)
						{
							if (instance.GameAccountId == null)
							{
								instance.GameAccountId = EntityId.DeserializeLengthDelimited(stream);
							}
							else
							{
								EntityId.DeserializeLengthDelimited(stream, instance.GameAccountId);
							}
							continue;
						}
						if (num2 == 26)
						{
							instance.Challenges.Add(Challenge.DeserializeLengthDelimited(stream));
							continue;
						}
						if (num2 == 50)
						{
							instance.Attributes.Add(Attribute.DeserializeLengthDelimited(stream));
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						uint field = key.Field;
						if (field != 0u)
						{
							ProtocolParser.SkipKey(stream, key);
							continue;
						}
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					case 40:
						instance.Timeout = ProtocolParser.ReadUInt64(stream);
						continue;
					}
					goto IL_9F;
				}
				if (limit >= 0L)
				{
					throw new EndOfStreamException();
				}
				return instance;
			}
			if (stream.get_Position() == limit)
			{
				return instance;
			}
			throw new ProtocolBufferException("Read past max limit");
		}

		public void Serialize(Stream stream)
		{
			SendChallengeToUserRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, SendChallengeToUserRequest instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.HasPeerId)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.PeerId.GetSerializedSize());
				ProcessId.Serialize(stream, instance.PeerId);
			}
			if (instance.HasGameAccountId)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteUInt32(stream, instance.GameAccountId.GetSerializedSize());
				EntityId.Serialize(stream, instance.GameAccountId);
			}
			if (instance.Challenges.get_Count() > 0)
			{
				using (List<Challenge>.Enumerator enumerator = instance.Challenges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Challenge current = enumerator.get_Current();
						stream.WriteByte(26);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						Challenge.Serialize(stream, current);
					}
				}
			}
			stream.WriteByte(37);
			binaryWriter.Write(instance.Context);
			if (instance.HasTimeout)
			{
				stream.WriteByte(40);
				ProtocolParser.WriteUInt64(stream, instance.Timeout);
			}
			if (instance.Attributes.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator2 = instance.Attributes.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Attribute current2 = enumerator2.get_Current();
						stream.WriteByte(50);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						Attribute.Serialize(stream, current2);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasPeerId)
			{
				num += 1u;
				uint serializedSize = this.PeerId.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.HasGameAccountId)
			{
				num += 1u;
				uint serializedSize2 = this.GameAccountId.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
			if (this.Challenges.get_Count() > 0)
			{
				using (List<Challenge>.Enumerator enumerator = this.Challenges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Challenge current = enumerator.get_Current();
						num += 1u;
						uint serializedSize3 = current.GetSerializedSize();
						num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
					}
				}
			}
			num += 4u;
			if (this.HasTimeout)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64(this.Timeout);
			}
			if (this.Attributes.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator2 = this.Attributes.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Attribute current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize4 = current2.GetSerializedSize();
						num += serializedSize4 + ProtocolParser.SizeOfUInt32(serializedSize4);
					}
				}
			}
			num += 1u;
			return num;
		}

		public void SetPeerId(ProcessId val)
		{
			this.PeerId = val;
		}

		public void SetGameAccountId(EntityId val)
		{
			this.GameAccountId = val;
		}

		public void AddChallenges(Challenge val)
		{
			this._Challenges.Add(val);
		}

		public void ClearChallenges()
		{
			this._Challenges.Clear();
		}

		public void SetChallenges(List<Challenge> val)
		{
			this.Challenges = val;
		}

		public void SetContext(uint val)
		{
			this.Context = val;
		}

		public void SetTimeout(ulong val)
		{
			this.Timeout = val;
		}

		public void AddAttributes(Attribute val)
		{
			this._Attributes.Add(val);
		}

		public void ClearAttributes()
		{
			this._Attributes.Clear();
		}

		public void SetAttributes(List<Attribute> val)
		{
			this.Attributes = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasPeerId)
			{
				num ^= this.PeerId.GetHashCode();
			}
			if (this.HasGameAccountId)
			{
				num ^= this.GameAccountId.GetHashCode();
			}
			using (List<Challenge>.Enumerator enumerator = this.Challenges.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Challenge current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			num ^= this.Context.GetHashCode();
			if (this.HasTimeout)
			{
				num ^= this.Timeout.GetHashCode();
			}
			using (List<Attribute>.Enumerator enumerator2 = this.Attributes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Attribute current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			SendChallengeToUserRequest sendChallengeToUserRequest = obj as SendChallengeToUserRequest;
			if (sendChallengeToUserRequest == null)
			{
				return false;
			}
			if (this.HasPeerId != sendChallengeToUserRequest.HasPeerId || (this.HasPeerId && !this.PeerId.Equals(sendChallengeToUserRequest.PeerId)))
			{
				return false;
			}
			if (this.HasGameAccountId != sendChallengeToUserRequest.HasGameAccountId || (this.HasGameAccountId && !this.GameAccountId.Equals(sendChallengeToUserRequest.GameAccountId)))
			{
				return false;
			}
			if (this.Challenges.get_Count() != sendChallengeToUserRequest.Challenges.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Challenges.get_Count(); i++)
			{
				if (!this.Challenges.get_Item(i).Equals(sendChallengeToUserRequest.Challenges.get_Item(i)))
				{
					return false;
				}
			}
			if (!this.Context.Equals(sendChallengeToUserRequest.Context))
			{
				return false;
			}
			if (this.HasTimeout != sendChallengeToUserRequest.HasTimeout || (this.HasTimeout && !this.Timeout.Equals(sendChallengeToUserRequest.Timeout)))
			{
				return false;
			}
			if (this.Attributes.get_Count() != sendChallengeToUserRequest.Attributes.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.Attributes.get_Count(); j++)
			{
				if (!this.Attributes.get_Item(j).Equals(sendChallengeToUserRequest.Attributes.get_Item(j)))
				{
					return false;
				}
			}
			return true;
		}

		public static SendChallengeToUserRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<SendChallengeToUserRequest>(bs, 0, -1);
		}
	}
}
