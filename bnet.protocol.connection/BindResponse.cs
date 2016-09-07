using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.connection
{
	public class BindResponse : IProtoBuf
	{
		private List<uint> _ImportedServiceId = new List<uint>();

		public List<uint> ImportedServiceId
		{
			get
			{
				return this._ImportedServiceId;
			}
			set
			{
				this._ImportedServiceId = value;
			}
		}

		public List<uint> ImportedServiceIdList
		{
			get
			{
				return this._ImportedServiceId;
			}
		}

		public int ImportedServiceIdCount
		{
			get
			{
				return this._ImportedServiceId.get_Count();
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
			BindResponse.Deserialize(stream, this);
		}

		public static BindResponse Deserialize(Stream stream, BindResponse instance)
		{
			return BindResponse.Deserialize(stream, instance, -1L);
		}

		public static BindResponse DeserializeLengthDelimited(Stream stream)
		{
			BindResponse bindResponse = new BindResponse();
			BindResponse.DeserializeLengthDelimited(stream, bindResponse);
			return bindResponse;
		}

		public static BindResponse DeserializeLengthDelimited(Stream stream, BindResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return BindResponse.Deserialize(stream, instance, num);
		}

		public static BindResponse Deserialize(Stream stream, BindResponse instance, long limit)
		{
			if (instance.ImportedServiceId == null)
			{
				instance.ImportedServiceId = new List<uint>();
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
							instance.ImportedServiceId.Add(ProtocolParser.ReadUInt32(stream));
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
			BindResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, BindResponse instance)
		{
			if (instance.ImportedServiceId.get_Count() > 0)
			{
				stream.WriteByte(10);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator = instance.ImportedServiceId.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator2 = instance.ImportedServiceId.GetEnumerator())
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
			if (this.ImportedServiceId.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator = this.ImportedServiceId.GetEnumerator())
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

		public void AddImportedServiceId(uint val)
		{
			this._ImportedServiceId.Add(val);
		}

		public void ClearImportedServiceId()
		{
			this._ImportedServiceId.Clear();
		}

		public void SetImportedServiceId(List<uint> val)
		{
			this.ImportedServiceId = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<uint>.Enumerator enumerator = this.ImportedServiceId.GetEnumerator())
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
			BindResponse bindResponse = obj as BindResponse;
			if (bindResponse == null)
			{
				return false;
			}
			if (this.ImportedServiceId.get_Count() != bindResponse.ImportedServiceId.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.ImportedServiceId.get_Count(); i++)
			{
				if (!this.ImportedServiceId.get_Item(i).Equals(bindResponse.ImportedServiceId.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static BindResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<BindResponse>(bs, 0, -1);
		}
	}
}
