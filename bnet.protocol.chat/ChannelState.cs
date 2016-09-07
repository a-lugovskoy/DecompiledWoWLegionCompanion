using System;
using System.IO;
using System.Text;

namespace bnet.protocol.chat
{
	public class ChannelState : IProtoBuf
	{
		public bool HasIdentity;

		private string _Identity;

		public bool HasProgram;

		private uint _Program;

		public bool HasLocale;

		private uint _Locale;

		public bool HasPublic;

		private bool _Public;

		public bool HasBucketIndex;

		private uint _BucketIndex;

		public string Identity
		{
			get
			{
				return this._Identity;
			}
			set
			{
				this._Identity = value;
				this.HasIdentity = (value != null);
			}
		}

		public uint Program
		{
			get
			{
				return this._Program;
			}
			set
			{
				this._Program = value;
				this.HasProgram = true;
			}
		}

		public uint Locale
		{
			get
			{
				return this._Locale;
			}
			set
			{
				this._Locale = value;
				this.HasLocale = true;
			}
		}

		public bool Public
		{
			get
			{
				return this._Public;
			}
			set
			{
				this._Public = value;
				this.HasPublic = true;
			}
		}

		public uint BucketIndex
		{
			get
			{
				return this._BucketIndex;
			}
			set
			{
				this._BucketIndex = value;
				this.HasBucketIndex = true;
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
			BinaryReader binaryReader = new BinaryReader(stream);
			instance.Public = false;
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num != -1)
				{
					int num2 = num;
					switch (num2)
					{
					case 29:
						instance.Locale = binaryReader.ReadUInt32();
						continue;
					case 30:
					case 31:
					{
						IL_7A:
						if (num2 == 10)
						{
							instance.Identity = ProtocolParser.ReadString(stream);
							continue;
						}
						if (num2 == 21)
						{
							instance.Program = binaryReader.ReadUInt32();
							continue;
						}
						if (num2 == 40)
						{
							instance.BucketIndex = ProtocolParser.ReadUInt32(stream);
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
					case 32:
						instance.Public = ProtocolParser.ReadBool(stream);
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
			ChannelState.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, ChannelState instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.HasIdentity)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteBytes(stream, Encoding.get_UTF8().GetBytes(instance.Identity));
			}
			if (instance.HasProgram)
			{
				stream.WriteByte(21);
				binaryWriter.Write(instance.Program);
			}
			if (instance.HasLocale)
			{
				stream.WriteByte(29);
				binaryWriter.Write(instance.Locale);
			}
			if (instance.HasPublic)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteBool(stream, instance.Public);
			}
			if (instance.HasBucketIndex)
			{
				stream.WriteByte(40);
				ProtocolParser.WriteUInt32(stream, instance.BucketIndex);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasIdentity)
			{
				num += 1u;
				uint byteCount = (uint)Encoding.get_UTF8().GetByteCount(this.Identity);
				num += ProtocolParser.SizeOfUInt32(byteCount) + byteCount;
			}
			if (this.HasProgram)
			{
				num += 1u;
				num += 4u;
			}
			if (this.HasLocale)
			{
				num += 1u;
				num += 4u;
			}
			if (this.HasPublic)
			{
				num += 1u;
				num += 1u;
			}
			if (this.HasBucketIndex)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.BucketIndex);
			}
			return num;
		}

		public void SetIdentity(string val)
		{
			this.Identity = val;
		}

		public void SetProgram(uint val)
		{
			this.Program = val;
		}

		public void SetLocale(uint val)
		{
			this.Locale = val;
		}

		public void SetPublic(bool val)
		{
			this.Public = val;
		}

		public void SetBucketIndex(uint val)
		{
			this.BucketIndex = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasIdentity)
			{
				num ^= this.Identity.GetHashCode();
			}
			if (this.HasProgram)
			{
				num ^= this.Program.GetHashCode();
			}
			if (this.HasLocale)
			{
				num ^= this.Locale.GetHashCode();
			}
			if (this.HasPublic)
			{
				num ^= this.Public.GetHashCode();
			}
			if (this.HasBucketIndex)
			{
				num ^= this.BucketIndex.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			ChannelState channelState = obj as ChannelState;
			return channelState != null && this.HasIdentity == channelState.HasIdentity && (!this.HasIdentity || this.Identity.Equals(channelState.Identity)) && this.HasProgram == channelState.HasProgram && (!this.HasProgram || this.Program.Equals(channelState.Program)) && this.HasLocale == channelState.HasLocale && (!this.HasLocale || this.Locale.Equals(channelState.Locale)) && this.HasPublic == channelState.HasPublic && (!this.HasPublic || this.Public.Equals(channelState.Public)) && this.HasBucketIndex == channelState.HasBucketIndex && (!this.HasBucketIndex || this.BucketIndex.Equals(channelState.BucketIndex));
		}

		public static ChannelState ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<ChannelState>(bs, 0, -1);
		}
	}
}
