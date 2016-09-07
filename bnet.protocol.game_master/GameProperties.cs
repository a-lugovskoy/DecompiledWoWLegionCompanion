using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class GameProperties : IProtoBuf
	{
		private List<Attribute> _CreationAttributes = new List<Attribute>();

		public bool HasFilter;

		private AttributeFilter _Filter;

		public bool HasCreate;

		private bool _Create;

		public bool HasOpen;

		private bool _Open;

		public bool HasProgramId;

		private uint _ProgramId;

		public List<Attribute> CreationAttributes
		{
			get
			{
				return this._CreationAttributes;
			}
			set
			{
				this._CreationAttributes = value;
			}
		}

		public List<Attribute> CreationAttributesList
		{
			get
			{
				return this._CreationAttributes;
			}
		}

		public int CreationAttributesCount
		{
			get
			{
				return this._CreationAttributes.get_Count();
			}
		}

		public AttributeFilter Filter
		{
			get
			{
				return this._Filter;
			}
			set
			{
				this._Filter = value;
				this.HasFilter = (value != null);
			}
		}

		public bool Create
		{
			get
			{
				return this._Create;
			}
			set
			{
				this._Create = value;
				this.HasCreate = true;
			}
		}

		public bool Open
		{
			get
			{
				return this._Open;
			}
			set
			{
				this._Open = value;
				this.HasOpen = true;
			}
		}

		public uint ProgramId
		{
			get
			{
				return this._ProgramId;
			}
			set
			{
				this._ProgramId = value;
				this.HasProgramId = true;
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
			GameProperties.Deserialize(stream, this);
		}

		public static GameProperties Deserialize(Stream stream, GameProperties instance)
		{
			return GameProperties.Deserialize(stream, instance, -1L);
		}

		public static GameProperties DeserializeLengthDelimited(Stream stream)
		{
			GameProperties gameProperties = new GameProperties();
			GameProperties.DeserializeLengthDelimited(stream, gameProperties);
			return gameProperties;
		}

		public static GameProperties DeserializeLengthDelimited(Stream stream, GameProperties instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GameProperties.Deserialize(stream, instance, num);
		}

		public static GameProperties Deserialize(Stream stream, GameProperties instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.CreationAttributes == null)
			{
				instance.CreationAttributes = new List<Attribute>();
			}
			instance.Create = false;
			instance.Open = true;
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
								if (num2 != 32)
								{
									if (num2 != 45)
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
										instance.ProgramId = binaryReader.ReadUInt32();
									}
								}
								else
								{
									instance.Open = ProtocolParser.ReadBool(stream);
								}
							}
							else
							{
								instance.Create = ProtocolParser.ReadBool(stream);
							}
						}
						else if (instance.Filter == null)
						{
							instance.Filter = AttributeFilter.DeserializeLengthDelimited(stream);
						}
						else
						{
							AttributeFilter.DeserializeLengthDelimited(stream, instance.Filter);
						}
					}
					else
					{
						instance.CreationAttributes.Add(Attribute.DeserializeLengthDelimited(stream));
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
			GameProperties.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GameProperties instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.CreationAttributes.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = instance.CreationAttributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						Attribute.Serialize(stream, current);
					}
				}
			}
			if (instance.HasFilter)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteUInt32(stream, instance.Filter.GetSerializedSize());
				AttributeFilter.Serialize(stream, instance.Filter);
			}
			if (instance.HasCreate)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteBool(stream, instance.Create);
			}
			if (instance.HasOpen)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteBool(stream, instance.Open);
			}
			if (instance.HasProgramId)
			{
				stream.WriteByte(45);
				binaryWriter.Write(instance.ProgramId);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.CreationAttributes.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = this.CreationAttributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.HasFilter)
			{
				num += 1u;
				uint serializedSize2 = this.Filter.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
			}
			if (this.HasCreate)
			{
				num += 1u;
				num += 1u;
			}
			if (this.HasOpen)
			{
				num += 1u;
				num += 1u;
			}
			if (this.HasProgramId)
			{
				num += 1u;
				num += 4u;
			}
			return num;
		}

		public void AddCreationAttributes(Attribute val)
		{
			this._CreationAttributes.Add(val);
		}

		public void ClearCreationAttributes()
		{
			this._CreationAttributes.Clear();
		}

		public void SetCreationAttributes(List<Attribute> val)
		{
			this.CreationAttributes = val;
		}

		public void SetFilter(AttributeFilter val)
		{
			this.Filter = val;
		}

		public void SetCreate(bool val)
		{
			this.Create = val;
		}

		public void SetOpen(bool val)
		{
			this.Open = val;
		}

		public void SetProgramId(uint val)
		{
			this.ProgramId = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<Attribute>.Enumerator enumerator = this.CreationAttributes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			if (this.HasFilter)
			{
				num ^= this.Filter.GetHashCode();
			}
			if (this.HasCreate)
			{
				num ^= this.Create.GetHashCode();
			}
			if (this.HasOpen)
			{
				num ^= this.Open.GetHashCode();
			}
			if (this.HasProgramId)
			{
				num ^= this.ProgramId.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GameProperties gameProperties = obj as GameProperties;
			if (gameProperties == null)
			{
				return false;
			}
			if (this.CreationAttributes.get_Count() != gameProperties.CreationAttributes.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.CreationAttributes.get_Count(); i++)
			{
				if (!this.CreationAttributes.get_Item(i).Equals(gameProperties.CreationAttributes.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasFilter == gameProperties.HasFilter && (!this.HasFilter || this.Filter.Equals(gameProperties.Filter)) && this.HasCreate == gameProperties.HasCreate && (!this.HasCreate || this.Create.Equals(gameProperties.Create)) && this.HasOpen == gameProperties.HasOpen && (!this.HasOpen || this.Open.Equals(gameProperties.Open)) && this.HasProgramId == gameProperties.HasProgramId && (!this.HasProgramId || this.ProgramId.Equals(gameProperties.ProgramId));
		}

		public static GameProperties ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GameProperties>(bs, 0, -1);
		}
	}
}
