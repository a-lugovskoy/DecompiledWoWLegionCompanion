using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.friends
{
	public class FriendInvitation : IProtoBuf
	{
		public bool HasFirstReceived;

		private bool _FirstReceived;

		private List<uint> _Role = new List<uint>();

		public bool FirstReceived
		{
			get
			{
				return this._FirstReceived;
			}
			set
			{
				this._FirstReceived = value;
				this.HasFirstReceived = true;
			}
		}

		public List<uint> Role
		{
			get
			{
				return this._Role;
			}
			set
			{
				this._Role = value;
			}
		}

		public List<uint> RoleList
		{
			get
			{
				return this._Role;
			}
		}

		public int RoleCount
		{
			get
			{
				return this._Role.get_Count();
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
			FriendInvitation.Deserialize(stream, this);
		}

		public static FriendInvitation Deserialize(Stream stream, FriendInvitation instance)
		{
			return FriendInvitation.Deserialize(stream, instance, -1L);
		}

		public static FriendInvitation DeserializeLengthDelimited(Stream stream)
		{
			FriendInvitation friendInvitation = new FriendInvitation();
			FriendInvitation.DeserializeLengthDelimited(stream, friendInvitation);
			return friendInvitation;
		}

		public static FriendInvitation DeserializeLengthDelimited(Stream stream, FriendInvitation instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return FriendInvitation.Deserialize(stream, instance, num);
		}

		public static FriendInvitation Deserialize(Stream stream, FriendInvitation instance, long limit)
		{
			instance.FirstReceived = false;
			if (instance.Role == null)
			{
				instance.Role = new List<uint>();
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
					if (num2 != 8)
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
							long num3 = (long)((ulong)ProtocolParser.ReadUInt32(stream));
							num3 += stream.get_Position();
							while (stream.get_Position() < num3)
							{
								instance.Role.Add(ProtocolParser.ReadUInt32(stream));
							}
							if (stream.get_Position() != num3)
							{
								throw new ProtocolBufferException("Read too many bytes in packed data");
							}
						}
					}
					else
					{
						instance.FirstReceived = ProtocolParser.ReadBool(stream);
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
			FriendInvitation.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, FriendInvitation instance)
		{
			if (instance.HasFirstReceived)
			{
				stream.WriteByte(8);
				ProtocolParser.WriteBool(stream, instance.FirstReceived);
			}
			if (instance.Role.get_Count() > 0)
			{
				stream.WriteByte(18);
				uint num = 0u;
				using (List<uint>.Enumerator enumerator = instance.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						num += ProtocolParser.SizeOfUInt32(current);
					}
				}
				ProtocolParser.WriteUInt32(stream, num);
				using (List<uint>.Enumerator enumerator2 = instance.Role.GetEnumerator())
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
			if (this.HasFirstReceived)
			{
				num += 1u;
				num += 1u;
			}
			if (this.Role.get_Count() > 0)
			{
				num += 1u;
				uint num2 = num;
				using (List<uint>.Enumerator enumerator = this.Role.GetEnumerator())
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

		public void SetFirstReceived(bool val)
		{
			this.FirstReceived = val;
		}

		public void AddRole(uint val)
		{
			this._Role.Add(val);
		}

		public void ClearRole()
		{
			this._Role.Clear();
		}

		public void SetRole(List<uint> val)
		{
			this.Role = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasFirstReceived)
			{
				num ^= this.FirstReceived.GetHashCode();
			}
			using (List<uint>.Enumerator enumerator = this.Role.GetEnumerator())
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
			FriendInvitation friendInvitation = obj as FriendInvitation;
			if (friendInvitation == null)
			{
				return false;
			}
			if (this.HasFirstReceived != friendInvitation.HasFirstReceived || (this.HasFirstReceived && !this.FirstReceived.Equals(friendInvitation.FirstReceived)))
			{
				return false;
			}
			if (this.Role.get_Count() != friendInvitation.Role.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Role.get_Count(); i++)
			{
				if (!this.Role.get_Item(i).Equals(friendInvitation.Role.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static FriendInvitation ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<FriendInvitation>(bs, 0, -1);
		}
	}
}
