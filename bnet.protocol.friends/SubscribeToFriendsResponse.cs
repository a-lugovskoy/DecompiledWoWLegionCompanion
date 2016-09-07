using bnet.protocol.invitation;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.friends
{
	public class SubscribeToFriendsResponse : IProtoBuf
	{
		public bool HasMaxFriends;

		private uint _MaxFriends;

		public bool HasMaxReceivedInvitations;

		private uint _MaxReceivedInvitations;

		public bool HasMaxSentInvitations;

		private uint _MaxSentInvitations;

		private List<Role> _Role = new List<Role>();

		private List<Friend> _Friends = new List<Friend>();

		private List<Invitation> _SentInvitations = new List<Invitation>();

		private List<Invitation> _ReceivedInvitations = new List<Invitation>();

		public uint MaxFriends
		{
			get
			{
				return this._MaxFriends;
			}
			set
			{
				this._MaxFriends = value;
				this.HasMaxFriends = true;
			}
		}

		public uint MaxReceivedInvitations
		{
			get
			{
				return this._MaxReceivedInvitations;
			}
			set
			{
				this._MaxReceivedInvitations = value;
				this.HasMaxReceivedInvitations = true;
			}
		}

		public uint MaxSentInvitations
		{
			get
			{
				return this._MaxSentInvitations;
			}
			set
			{
				this._MaxSentInvitations = value;
				this.HasMaxSentInvitations = true;
			}
		}

		public List<Role> Role
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

		public List<Role> RoleList
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

		public List<Friend> Friends
		{
			get
			{
				return this._Friends;
			}
			set
			{
				this._Friends = value;
			}
		}

		public List<Friend> FriendsList
		{
			get
			{
				return this._Friends;
			}
		}

		public int FriendsCount
		{
			get
			{
				return this._Friends.get_Count();
			}
		}

		public List<Invitation> SentInvitations
		{
			get
			{
				return this._SentInvitations;
			}
			set
			{
				this._SentInvitations = value;
			}
		}

		public List<Invitation> SentInvitationsList
		{
			get
			{
				return this._SentInvitations;
			}
		}

		public int SentInvitationsCount
		{
			get
			{
				return this._SentInvitations.get_Count();
			}
		}

		public List<Invitation> ReceivedInvitations
		{
			get
			{
				return this._ReceivedInvitations;
			}
			set
			{
				this._ReceivedInvitations = value;
			}
		}

		public List<Invitation> ReceivedInvitationsList
		{
			get
			{
				return this._ReceivedInvitations;
			}
		}

		public int ReceivedInvitationsCount
		{
			get
			{
				return this._ReceivedInvitations.get_Count();
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
			SubscribeToFriendsResponse.Deserialize(stream, this);
		}

		public static SubscribeToFriendsResponse Deserialize(Stream stream, SubscribeToFriendsResponse instance)
		{
			return SubscribeToFriendsResponse.Deserialize(stream, instance, -1L);
		}

		public static SubscribeToFriendsResponse DeserializeLengthDelimited(Stream stream)
		{
			SubscribeToFriendsResponse subscribeToFriendsResponse = new SubscribeToFriendsResponse();
			SubscribeToFriendsResponse.DeserializeLengthDelimited(stream, subscribeToFriendsResponse);
			return subscribeToFriendsResponse;
		}

		public static SubscribeToFriendsResponse DeserializeLengthDelimited(Stream stream, SubscribeToFriendsResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return SubscribeToFriendsResponse.Deserialize(stream, instance, num);
		}

		public static SubscribeToFriendsResponse Deserialize(Stream stream, SubscribeToFriendsResponse instance, long limit)
		{
			if (instance.Role == null)
			{
				instance.Role = new List<Role>();
			}
			if (instance.Friends == null)
			{
				instance.Friends = new List<Friend>();
			}
			if (instance.SentInvitations == null)
			{
				instance.SentInvitations = new List<Invitation>();
			}
			if (instance.ReceivedInvitations == null)
			{
				instance.ReceivedInvitations = new List<Invitation>();
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
						if (num2 != 16)
						{
							if (num2 != 24)
							{
								if (num2 != 34)
								{
									if (num2 != 42)
									{
										if (num2 != 50)
										{
											if (num2 != 58)
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
												instance.ReceivedInvitations.Add(Invitation.DeserializeLengthDelimited(stream));
											}
										}
										else
										{
											instance.SentInvitations.Add(Invitation.DeserializeLengthDelimited(stream));
										}
									}
									else
									{
										instance.Friends.Add(Friend.DeserializeLengthDelimited(stream));
									}
								}
								else
								{
									instance.Role.Add(bnet.protocol.Role.DeserializeLengthDelimited(stream));
								}
							}
							else
							{
								instance.MaxSentInvitations = ProtocolParser.ReadUInt32(stream);
							}
						}
						else
						{
							instance.MaxReceivedInvitations = ProtocolParser.ReadUInt32(stream);
						}
					}
					else
					{
						instance.MaxFriends = ProtocolParser.ReadUInt32(stream);
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
			SubscribeToFriendsResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, SubscribeToFriendsResponse instance)
		{
			if (instance.HasMaxFriends)
			{
				stream.WriteByte(8);
				ProtocolParser.WriteUInt32(stream, instance.MaxFriends);
			}
			if (instance.HasMaxReceivedInvitations)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, instance.MaxReceivedInvitations);
			}
			if (instance.HasMaxSentInvitations)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt32(stream, instance.MaxSentInvitations);
			}
			if (instance.Role.get_Count() > 0)
			{
				using (List<Role>.Enumerator enumerator = instance.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Role current = enumerator.get_Current();
						stream.WriteByte(34);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.Role.Serialize(stream, current);
					}
				}
			}
			if (instance.Friends.get_Count() > 0)
			{
				using (List<Friend>.Enumerator enumerator2 = instance.Friends.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Friend current2 = enumerator2.get_Current();
						stream.WriteByte(42);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						Friend.Serialize(stream, current2);
					}
				}
			}
			if (instance.SentInvitations.get_Count() > 0)
			{
				using (List<Invitation>.Enumerator enumerator3 = instance.SentInvitations.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Invitation current3 = enumerator3.get_Current();
						stream.WriteByte(50);
						ProtocolParser.WriteUInt32(stream, current3.GetSerializedSize());
						Invitation.Serialize(stream, current3);
					}
				}
			}
			if (instance.ReceivedInvitations.get_Count() > 0)
			{
				using (List<Invitation>.Enumerator enumerator4 = instance.ReceivedInvitations.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Invitation current4 = enumerator4.get_Current();
						stream.WriteByte(58);
						ProtocolParser.WriteUInt32(stream, current4.GetSerializedSize());
						Invitation.Serialize(stream, current4);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasMaxFriends)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.MaxFriends);
			}
			if (this.HasMaxReceivedInvitations)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.MaxReceivedInvitations);
			}
			if (this.HasMaxSentInvitations)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.MaxSentInvitations);
			}
			if (this.Role.get_Count() > 0)
			{
				using (List<Role>.Enumerator enumerator = this.Role.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Role current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.Friends.get_Count() > 0)
			{
				using (List<Friend>.Enumerator enumerator2 = this.Friends.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Friend current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize2 = current2.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			if (this.SentInvitations.get_Count() > 0)
			{
				using (List<Invitation>.Enumerator enumerator3 = this.SentInvitations.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Invitation current3 = enumerator3.get_Current();
						num += 1u;
						uint serializedSize3 = current3.GetSerializedSize();
						num += serializedSize3 + ProtocolParser.SizeOfUInt32(serializedSize3);
					}
				}
			}
			if (this.ReceivedInvitations.get_Count() > 0)
			{
				using (List<Invitation>.Enumerator enumerator4 = this.ReceivedInvitations.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Invitation current4 = enumerator4.get_Current();
						num += 1u;
						uint serializedSize4 = current4.GetSerializedSize();
						num += serializedSize4 + ProtocolParser.SizeOfUInt32(serializedSize4);
					}
				}
			}
			return num;
		}

		public void SetMaxFriends(uint val)
		{
			this.MaxFriends = val;
		}

		public void SetMaxReceivedInvitations(uint val)
		{
			this.MaxReceivedInvitations = val;
		}

		public void SetMaxSentInvitations(uint val)
		{
			this.MaxSentInvitations = val;
		}

		public void AddRole(Role val)
		{
			this._Role.Add(val);
		}

		public void ClearRole()
		{
			this._Role.Clear();
		}

		public void SetRole(List<Role> val)
		{
			this.Role = val;
		}

		public void AddFriends(Friend val)
		{
			this._Friends.Add(val);
		}

		public void ClearFriends()
		{
			this._Friends.Clear();
		}

		public void SetFriends(List<Friend> val)
		{
			this.Friends = val;
		}

		public void AddSentInvitations(Invitation val)
		{
			this._SentInvitations.Add(val);
		}

		public void ClearSentInvitations()
		{
			this._SentInvitations.Clear();
		}

		public void SetSentInvitations(List<Invitation> val)
		{
			this.SentInvitations = val;
		}

		public void AddReceivedInvitations(Invitation val)
		{
			this._ReceivedInvitations.Add(val);
		}

		public void ClearReceivedInvitations()
		{
			this._ReceivedInvitations.Clear();
		}

		public void SetReceivedInvitations(List<Invitation> val)
		{
			this.ReceivedInvitations = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasMaxFriends)
			{
				num ^= this.MaxFriends.GetHashCode();
			}
			if (this.HasMaxReceivedInvitations)
			{
				num ^= this.MaxReceivedInvitations.GetHashCode();
			}
			if (this.HasMaxSentInvitations)
			{
				num ^= this.MaxSentInvitations.GetHashCode();
			}
			using (List<Role>.Enumerator enumerator = this.Role.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Role current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<Friend>.Enumerator enumerator2 = this.Friends.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Friend current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			using (List<Invitation>.Enumerator enumerator3 = this.SentInvitations.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Invitation current3 = enumerator3.get_Current();
					num ^= current3.GetHashCode();
				}
			}
			using (List<Invitation>.Enumerator enumerator4 = this.ReceivedInvitations.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					Invitation current4 = enumerator4.get_Current();
					num ^= current4.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			SubscribeToFriendsResponse subscribeToFriendsResponse = obj as SubscribeToFriendsResponse;
			if (subscribeToFriendsResponse == null)
			{
				return false;
			}
			if (this.HasMaxFriends != subscribeToFriendsResponse.HasMaxFriends || (this.HasMaxFriends && !this.MaxFriends.Equals(subscribeToFriendsResponse.MaxFriends)))
			{
				return false;
			}
			if (this.HasMaxReceivedInvitations != subscribeToFriendsResponse.HasMaxReceivedInvitations || (this.HasMaxReceivedInvitations && !this.MaxReceivedInvitations.Equals(subscribeToFriendsResponse.MaxReceivedInvitations)))
			{
				return false;
			}
			if (this.HasMaxSentInvitations != subscribeToFriendsResponse.HasMaxSentInvitations || (this.HasMaxSentInvitations && !this.MaxSentInvitations.Equals(subscribeToFriendsResponse.MaxSentInvitations)))
			{
				return false;
			}
			if (this.Role.get_Count() != subscribeToFriendsResponse.Role.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Role.get_Count(); i++)
			{
				if (!this.Role.get_Item(i).Equals(subscribeToFriendsResponse.Role.get_Item(i)))
				{
					return false;
				}
			}
			if (this.Friends.get_Count() != subscribeToFriendsResponse.Friends.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.Friends.get_Count(); j++)
			{
				if (!this.Friends.get_Item(j).Equals(subscribeToFriendsResponse.Friends.get_Item(j)))
				{
					return false;
				}
			}
			if (this.SentInvitations.get_Count() != subscribeToFriendsResponse.SentInvitations.get_Count())
			{
				return false;
			}
			for (int k = 0; k < this.SentInvitations.get_Count(); k++)
			{
				if (!this.SentInvitations.get_Item(k).Equals(subscribeToFriendsResponse.SentInvitations.get_Item(k)))
				{
					return false;
				}
			}
			if (this.ReceivedInvitations.get_Count() != subscribeToFriendsResponse.ReceivedInvitations.get_Count())
			{
				return false;
			}
			for (int l = 0; l < this.ReceivedInvitations.get_Count(); l++)
			{
				if (!this.ReceivedInvitations.get_Item(l).Equals(subscribeToFriendsResponse.ReceivedInvitations.get_Item(l)))
				{
					return false;
				}
			}
			return true;
		}

		public static SubscribeToFriendsResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<SubscribeToFriendsResponse>(bs, 0, -1);
		}
	}
}
