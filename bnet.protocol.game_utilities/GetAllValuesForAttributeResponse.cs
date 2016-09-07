using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_utilities
{
	public class GetAllValuesForAttributeResponse : IProtoBuf
	{
		private List<Variant> _AttributeValue = new List<Variant>();

		public List<Variant> AttributeValue
		{
			get
			{
				return this._AttributeValue;
			}
			set
			{
				this._AttributeValue = value;
			}
		}

		public void Deserialize(Stream stream)
		{
			GetAllValuesForAttributeResponse.Deserialize(stream, this);
		}

		public static GetAllValuesForAttributeResponse Deserialize(Stream stream, GetAllValuesForAttributeResponse instance)
		{
			return GetAllValuesForAttributeResponse.Deserialize(stream, instance, -1L);
		}

		public static GetAllValuesForAttributeResponse DeserializeLengthDelimited(Stream stream)
		{
			GetAllValuesForAttributeResponse getAllValuesForAttributeResponse = new GetAllValuesForAttributeResponse();
			GetAllValuesForAttributeResponse.DeserializeLengthDelimited(stream, getAllValuesForAttributeResponse);
			return getAllValuesForAttributeResponse;
		}

		public static GetAllValuesForAttributeResponse DeserializeLengthDelimited(Stream stream, GetAllValuesForAttributeResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GetAllValuesForAttributeResponse.Deserialize(stream, instance, num);
		}

		public static GetAllValuesForAttributeResponse Deserialize(Stream stream, GetAllValuesForAttributeResponse instance, long limit)
		{
			if (instance.AttributeValue == null)
			{
				instance.AttributeValue = new List<Variant>();
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
						instance.AttributeValue.Add(Variant.DeserializeLengthDelimited(stream));
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
			GetAllValuesForAttributeResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GetAllValuesForAttributeResponse instance)
		{
			if (instance.AttributeValue.get_Count() > 0)
			{
				using (List<Variant>.Enumerator enumerator = instance.AttributeValue.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Variant current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						Variant.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.AttributeValue.get_Count() > 0)
			{
				using (List<Variant>.Enumerator enumerator = this.AttributeValue.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Variant current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<Variant>.Enumerator enumerator = this.AttributeValue.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Variant current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GetAllValuesForAttributeResponse getAllValuesForAttributeResponse = obj as GetAllValuesForAttributeResponse;
			if (getAllValuesForAttributeResponse == null)
			{
				return false;
			}
			if (this.AttributeValue.get_Count() != getAllValuesForAttributeResponse.AttributeValue.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.AttributeValue.get_Count(); i++)
			{
				if (!this.AttributeValue.get_Item(i).Equals(getAllValuesForAttributeResponse.AttributeValue.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}
	}
}
