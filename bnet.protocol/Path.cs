using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol
{
	public class Path : IProtoBuf
	{
		private List<uint> _Ordinal = new List<uint>();

		public List<uint> Ordinal
		{
			get
			{
				return this._Ordinal;
			}
			set
			{
				this._Ordinal = value;
			}
		}

		public List<uint> OrdinalList
		{
			get
			{
				return this._Ordinal;
			}
		}

		public int OrdinalCount
		{
			get
			{
				return this._Ordinal.get_Count();
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
			Path.Deserialize(stream, this);
		}

		public static Path Deserialize(Stream stream, Path instance)
		{
			return Path.Deserialize(stream, instance, -1L);
		}

		public static Path DeserializeLengthDelimited(Stream stream)
		{
			Path path = new Path();
			Path.DeserializeLengthDelimited(stream, path);
			return path;
		}

		public static Path DeserializeLengthDelimited(Stream stream, Path instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return Path.Deserialize(stream, instance, num);
		}

		public static Path Deserialize(Stream stream, Path instance, long limit)
		{
			if (instance.Ordinal == null)
			{
				instance.Ordinal = new List<uint>();
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
							instance.Ordinal.Add(ProtocolParser.ReadUInt32(stream));
						}
						if (stream.get_Position() != num3)
						{
							throw new ProtocolBufferException("Read too many bytes in packed data");
						}
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
			Path.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, Path instance)
		{
			if (instance.Ordinal.get_Count() > 0)
			{
				stream.WriteByte(10);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator = instance.Ordinal.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator2 = instance.Ordinal.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = enumerator2.get_Current();
						ProtocolParser.WriteUInt32(stream, current2);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Ordinal.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator = this.Ordinal.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			return num;
		}

		public void AddOrdinal(uint val)
		{
			this._Ordinal.Add(val);
		}

		public void ClearOrdinal()
		{
			this._Ordinal.Clear();
		}

		public void SetOrdinal(List<uint> val)
		{
			this.Ordinal = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<uint>.Enumerator enumerator = this.Ordinal.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			Path path = obj as Path;
			if (path == null)
			{
				return false;
			}
			if (this.Ordinal.get_Count() != path.Ordinal.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Ordinal.get_Count(); i++)
			{
				if (!this.Ordinal.get_Item(i).Equals(path.Ordinal.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static Path ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<Path>(bs, 0, -1);
		}
	}
}
