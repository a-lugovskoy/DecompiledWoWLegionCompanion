using System;
using System.IO;

namespace bnet.protocol.challenge
{
	public class ChallengePickedRequest : IProtoBuf
	{
		public bool HasId;

		private uint _Id;

		public bool HasNewChallengeProtocol;

		private bool _NewChallengeProtocol;

		public uint Challenge
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

		public bool NewChallengeProtocol
		{
			get
			{
				return this._NewChallengeProtocol;
			}
			set
			{
				this._NewChallengeProtocol = value;
				this.HasNewChallengeProtocol = true;
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
			ChallengePickedRequest.Deserialize(stream, this);
		}

		public static ChallengePickedRequest Deserialize(Stream stream, ChallengePickedRequest instance)
		{
			return ChallengePickedRequest.Deserialize(stream, instance, -1L);
		}

		public static ChallengePickedRequest DeserializeLengthDelimited(Stream stream)
		{
			ChallengePickedRequest challengePickedRequest = new ChallengePickedRequest();
			ChallengePickedRequest.DeserializeLengthDelimited(stream, challengePickedRequest);
			return challengePickedRequest;
		}

		public static ChallengePickedRequest DeserializeLengthDelimited(Stream stream, ChallengePickedRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return ChallengePickedRequest.Deserialize(stream, instance, num);
		}

		public static ChallengePickedRequest Deserialize(Stream stream, ChallengePickedRequest instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			instance.NewChallengeProtocol = false;
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num != -1)
				{
					int num2 = num;
					switch (num2)
					{
					case 13:
						instance.Challenge = binaryReader.ReadUInt32();
						continue;
					case 14:
					case 15:
					{
						IL_7A:
						if (num2 == 24)
						{
							instance.NewChallengeProtocol = ProtocolParser.ReadBool(stream);
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
					case 16:
						instance.Id = ProtocolParser.ReadUInt32(stream);
						continue;
					}
					goto IL_7A;
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
			ChallengePickedRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ChallengePickedRequest instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			stream.WriteByte(13);
			binaryWriter.Write(instance.Challenge);
			if (instance.HasId)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, instance.Id);
			}
			if (instance.HasNewChallengeProtocol)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteBool(stream, instance.NewChallengeProtocol);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += 4u;
			if (this.HasId)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.Id);
			}
			if (this.HasNewChallengeProtocol)
			{
				num += 1u;
				num += 1u;
			}
			return num + 1u;
		}

		public void SetChallenge(uint val)
		{
			this.Challenge = val;
		}

		public void SetId(uint val)
		{
			this.Id = val;
		}

		public void SetNewChallengeProtocol(bool val)
		{
			this.NewChallengeProtocol = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.Challenge.GetHashCode();
			if (this.HasId)
			{
				num ^= this.Id.GetHashCode();
			}
			if (this.HasNewChallengeProtocol)
			{
				num ^= this.NewChallengeProtocol.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ChallengePickedRequest challengePickedRequest = obj as ChallengePickedRequest;
			return challengePickedRequest != null && this.Challenge.Equals(challengePickedRequest.Challenge) && this.HasId == challengePickedRequest.HasId && (!this.HasId || this.Id.Equals(challengePickedRequest.Id)) && this.HasNewChallengeProtocol == challengePickedRequest.HasNewChallengeProtocol && (!this.HasNewChallengeProtocol || this.NewChallengeProtocol.Equals(challengePickedRequest.NewChallengeProtocol));
		}

		public static ChallengePickedRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ChallengePickedRequest>(bs, 0, -1);
		}
	}
}
