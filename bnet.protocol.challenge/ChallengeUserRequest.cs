using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.challenge
{
	public class ChallengeUserRequest : IProtoBuf
	{
		private List<Challenge> _Challenges = new List<Challenge>();

		public bool HasId;

		private uint _Id;

		public bool HasDeadline;

		private ulong _Deadline;

		private List<Attribute> _Attributes = new List<Attribute>();

		public bool HasGameAccountId;

		private EntityId _GameAccountId;

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

		public uint Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				this._Id = value;
				this.HasId = true;
			}
		}

		public ulong Deadline
		{
			get
			{
				return this._Deadline;
			}
			set
			{
				this._Deadline = value;
				this.HasDeadline = true;
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

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			ChallengeUserRequest.Deserialize(stream, this);
		}

		public static ChallengeUserRequest Deserialize(Stream stream, ChallengeUserRequest instance)
		{
			return ChallengeUserRequest.Deserialize(stream, instance, -1L);
		}

		public static ChallengeUserRequest DeserializeLengthDelimited(Stream stream)
		{
			ChallengeUserRequest challengeUserRequest = new ChallengeUserRequest();
			ChallengeUserRequest.DeserializeLengthDelimited(stream, challengeUserRequest);
			return challengeUserRequest;
		}

		public static ChallengeUserRequest DeserializeLengthDelimited(Stream stream, ChallengeUserRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ChallengeUserRequest.Deserialize(stream, instance, num);
		}

		public static ChallengeUserRequest Deserialize(Stream stream, ChallengeUserRequest instance, long limit)
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
					case 21:
						instance.Context = binaryReader.ReadUInt32();
						continue;
					case 22:
					case 23:
					{
						IL_9F:
						if (num2 == 10)
						{
							instance.Challenges.Add(Challenge.DeserializeLengthDelimited(stream));
							continue;
						}
						if (num2 == 32)
						{
							instance.Deadline = ProtocolParser.ReadUInt64(stream);
							continue;
						}
						if (num2 == 42)
						{
							instance.Attributes.Add(Attribute.DeserializeLengthDelimited(stream));
							continue;
						}
						if (num2 == 50)
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
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						uint field = key.Field;
						if (field != 0u)
						{
							ProtocolParser.SkipKey(stream, key);
							continue;
						}
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					case 24:
						instance.Id = ProtocolParser.ReadUInt32(stream);
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
			ChallengeUserRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ChallengeUserRequest instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.Challenges.get_Count() > 0)
			{
				using (List<Challenge>.Enumerator enumerator = instance.Challenges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Challenge current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						Challenge.Serialize(stream, current);
					}
				}
			}
			stream.WriteByte(21);
			binaryWriter.Write(instance.Context);
			if (instance.HasId)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt32(stream, instance.Id);
			}
			if (instance.HasDeadline)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteUInt64(stream, instance.Deadline);
			}
			if (instance.Attributes.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator2 = instance.Attributes.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Attribute current2 = enumerator2.get_Current();
						stream.WriteByte(42);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						Attribute.Serialize(stream, current2);
					}
				}
			}
			if (instance.HasGameAccountId)
			{
				stream.WriteByte(50);
				ProtocolParser.WriteUInt32(stream, instance.GameAccountId.GetSerializedSize());
				EntityId.Serialize(stream, instance.GameAccountId);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Challenges.get_Count() > 0)
			{
				using (List<Challenge>.Enumerator enumerator = this.Challenges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Challenge current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			num += 4u;
			if (this.HasId)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.Id);
			}
			if (this.HasDeadline)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt64(this.Deadline);
			}
			if (this.Attributes.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator2 = this.Attributes.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Attribute current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize2 = current2.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			if (this.HasGameAccountId)
			{
				num += 1u;
				uint serializedSize3 = this.GameAccountId.GetSerializedSize();
				num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
			}
			num += 1u;
			return num;
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

		public void SetId(uint val)
		{
			this.Id = val;
		}

		public void SetDeadline(ulong val)
		{
			this.Deadline = val;
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

		public void SetGameAccountId(EntityId val)
		{
			this.GameAccountId = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<Challenge>.Enumerator enumerator = this.Challenges.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Challenge current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			num ^= this.Context.GetHashCode();
			if (this.HasId)
			{
				num ^= this.Id.GetHashCode();
			}
			if (this.HasDeadline)
			{
				num ^= this.Deadline.GetHashCode();
			}
			using (List<Attribute>.Enumerator enumerator2 = this.Attributes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Attribute current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			if (this.HasGameAccountId)
			{
				num ^= this.GameAccountId.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ChallengeUserRequest challengeUserRequest = obj as ChallengeUserRequest;
			if (challengeUserRequest == null)
			{
				return false;
			}
			if (this.Challenges.get_Count() != challengeUserRequest.Challenges.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Challenges.get_Count(); i++)
			{
				if (!this.Challenges.get_Item(i).Equals(challengeUserRequest.Challenges.get_Item(i)))
				{
					return false;
				}
			}
			if (!this.Context.Equals(challengeUserRequest.Context))
			{
				return false;
			}
			if (this.HasId != challengeUserRequest.HasId || (this.HasId && !this.Id.Equals(challengeUserRequest.Id)))
			{
				return false;
			}
			if (this.HasDeadline != challengeUserRequest.HasDeadline || (this.HasDeadline && !this.Deadline.Equals(challengeUserRequest.Deadline)))
			{
				return false;
			}
			if (this.Attributes.get_Count() != challengeUserRequest.Attributes.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.Attributes.get_Count(); j++)
			{
				if (!this.Attributes.get_Item(j).Equals(challengeUserRequest.Attributes.get_Item(j)))
				{
					return false;
				}
			}
			return this.HasGameAccountId == challengeUserRequest.HasGameAccountId && (!this.HasGameAccountId || this.GameAccountId.Equals(challengeUserRequest.GameAccountId));
		}

		public static ChallengeUserRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ChallengeUserRequest>(bs, 0, -1);
		}
	}
}
