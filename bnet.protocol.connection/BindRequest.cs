using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.connection
{
	public class BindRequest : IProtoBuf
	{
		private List<uint> _ImportedServiceHash = new List<uint>();

		private List<BoundService> _ExportedService = new List<BoundService>();

		public List<uint> ImportedServiceHash
		{
			get
			{
				return this._ImportedServiceHash;
			}
			set
			{
				this._ImportedServiceHash = value;
			}
		}

		public List<uint> ImportedServiceHashList
		{
			get
			{
				return this._ImportedServiceHash;
			}
		}

		public int ImportedServiceHashCount
		{
			get
			{
				return this._ImportedServiceHash.get_Count();
			}
		}

		public List<BoundService> ExportedService
		{
			get
			{
				return this._ExportedService;
			}
			set
			{
				this._ExportedService = value;
			}
		}

		public List<BoundService> ExportedServiceList
		{
			get
			{
				return this._ExportedService;
			}
		}

		public int ExportedServiceCount
		{
			get
			{
				return this._ExportedService.get_Count();
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
			BindRequest.Deserialize(stream, this);
		}

		public static BindRequest Deserialize(Stream stream, BindRequest instance)
		{
			return BindRequest.Deserialize(stream, instance, -1L);
		}

		public static BindRequest DeserializeLengthDelimited(Stream stream)
		{
			BindRequest bindRequest = new BindRequest();
			BindRequest.DeserializeLengthDelimited(stream, bindRequest);
			return bindRequest;
		}

		public static BindRequest DeserializeLengthDelimited(Stream stream, BindRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return BindRequest.Deserialize(stream, instance, num);
		}

		public static BindRequest Deserialize(Stream stream, BindRequest instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.ImportedServiceHash == null)
			{
				instance.ImportedServiceHash = new List<uint>();
			}
			if (instance.ExportedService == null)
			{
				instance.ExportedService = new List<BoundService>();
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
							instance.ExportedService.Add(BoundService.DeserializeLengthDelimited(stream));
						}
					}
					else
					{
						long num3 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
						num3 += stream.get_Position();
						while (stream.get_Position() < num3)
						{
							instance.ImportedServiceHash.Add(binaryReader.ReadUInt32());
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
			BindRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, BindRequest instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.ImportedServiceHash.get_Count() > 0)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, (uint)(4 * instance.ImportedServiceHash.get_Count()));
				using (List<uint>.Enumerator enumerator = instance.ImportedServiceHash.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						binaryWriter.Write(current);
					}
				}
			}
			if (instance.ExportedService.get_Count() > 0)
			{
				using (List<BoundService>.Enumerator enumerator2 = instance.ExportedService.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BoundService current2 = enumerator2.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						BoundService.Serialize(stream, current2);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.ImportedServiceHash.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator = this.ImportedServiceHash.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += 4u;
					}
				}
				num += ProtocolParser.SizeOfUInt32(num - num2);
			}
			if (this.ExportedService.get_Count() > 0)
			{
				using (List<BoundService>.Enumerator enumerator2 = this.ExportedService.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BoundService current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize = current2.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddImportedServiceHash(uint val)
		{
			this._ImportedServiceHash.Add(val);
		}

		public void ClearImportedServiceHash()
		{
			this._ImportedServiceHash.Clear();
		}

		public void SetImportedServiceHash(List<uint> val)
		{
			this.ImportedServiceHash = val;
		}

		public void AddExportedService(BoundService val)
		{
			this._ExportedService.Add(val);
		}

		public void ClearExportedService()
		{
			this._ExportedService.Clear();
		}

		public void SetExportedService(List<BoundService> val)
		{
			this.ExportedService = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<uint>.Enumerator enumerator = this.ImportedServiceHash.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<BoundService>.Enumerator enumerator2 = this.ExportedService.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BoundService current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			BindRequest bindRequest = obj as BindRequest;
			if (bindRequest == null)
			{
				return false;
			}
			if (this.ImportedServiceHash.get_Count() != bindRequest.ImportedServiceHash.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.ImportedServiceHash.get_Count(); i++)
			{
				if (!this.ImportedServiceHash.get_Item(i).Equals(bindRequest.ImportedServiceHash.get_Item(i)))
				{
					return false;
				}
			}
			if (this.ExportedService.get_Count() != bindRequest.ExportedService.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.ExportedService.get_Count(); j++)
			{
				if (!this.ExportedService.get_Item(j).Equals(bindRequest.ExportedService.get_Item(j)))
				{
					return false;
				}
			}
			return true;
		}

		public static BindRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<BindRequest>(bs, 0, -1);
		}
	}
}
