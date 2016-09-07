using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_master
{
	public class Player : IProtoBuf
	{
		public bool HasIdentity;

		private Identity _Identity;

		private List<Attribute> _Attribute = new List<Attribute>();

		public Identity Identity
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

		public List<Attribute> Attribute
		{
			get
			{
				return this._Attribute;
			}
			set
			{
				this._Attribute = value;
			}
		}

		public List<Attribute> AttributeList
		{
			get
			{
				return this._Attribute;
			}
		}

		public int AttributeCount
		{
			get
			{
				return this._Attribute.get_Count();
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
			Player.Deserialize(stream, this);
		}

		public static Player Deserialize(Stream stream, Player instance)
		{
			return Player.Deserialize(stream, instance, -1L);
		}

		public static Player DeserializeLengthDelimited(Stream stream)
		{
			Player player = new Player();
			Player.DeserializeLengthDelimited(stream, player);
			return player;
		}

		public static Player DeserializeLengthDelimited(Stream stream, Player instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return Player.Deserialize(stream, instance, num);
		}

		public static Player Deserialize(Stream stream, Player instance, long limit)
		{
			if (instance.Attribute == null)
			{
				instance.Attribute = new List<Attribute>();
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
							instance.Attribute.Add(bnet.protocol.attribute.Attribute.DeserializeLengthDelimited(stream));
						}
					}
					else if (instance.Identity == null)
					{
						instance.Identity = Identity.DeserializeLengthDelimited(stream);
					}
					else
					{
						Identity.DeserializeLengthDelimited(stream, instance.Identity);
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
			Player.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, Player instance)
		{
			if (instance.HasIdentity)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteUInt32(stream, instance.Identity.GetSerializedSize());
				Identity.Serialize(stream, instance.Identity);
			}
			if (instance.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = instance.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.attribute.Attribute.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasIdentity)
			{
				num += 1u;
				uint serializedSize = this.Identity.GetSerializedSize();
				num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			}
			if (this.Attribute.get_Count() > 0)
			{
				using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			return num;
		}

		public void SetIdentity(Identity val)
		{
			this.Identity = val;
		}

		public void AddAttribute(Attribute val)
		{
			this._Attribute.Add(val);
		}

		public void ClearAttribute()
		{
			this._Attribute.Clear();
		}

		public void SetAttribute(List<Attribute> val)
		{
			this.Attribute = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasIdentity)
			{
				num ^= this.Identity.GetHashCode();
			}
			using (List<Attribute>.Enumerator enumerator = this.Attribute.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			Player player = obj as Player;
			if (player == null)
			{
				return false;
			}
			if (this.HasIdentity != player.HasIdentity || (this.HasIdentity && !this.Identity.Equals(player.Identity)))
			{
				return false;
			}
			if (this.Attribute.get_Count() != player.Attribute.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Attribute.get_Count(); i++)
			{
				if (!this.Attribute.get_Item(i).Equals(player.Attribute.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static Player ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<Player>(bs, 0, -1);
		}
	}
}
