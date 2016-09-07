using bnet.protocol.invitation;
using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.channel_invitation
{
	public class SubscribeResponse : IProtoBuf
	{
		private List<InvitationCollection> _Collection = new List<InvitationCollection>();

		private List<Invitation> _ReceivedInvitation = new List<Invitation>();

		public List<InvitationCollection> Collection
		{
			get
			{
				return this._Collection;
			}
			set
			{
				this._Collection = value;
			}
		}

		public List<InvitationCollection> CollectionList
		{
			get
			{
				return this._Collection;
			}
		}

		public int CollectionCount
		{
			get
			{
				return this._Collection.get_Count();
			}
		}

		public List<Invitation> ReceivedInvitation
		{
			get
			{
				return this._ReceivedInvitation;
			}
			set
			{
				this._ReceivedInvitation = value;
			}
		}

		public List<Invitation> ReceivedInvitationList
		{
			get
			{
				return this._ReceivedInvitation;
			}
		}

		public int ReceivedInvitationCount
		{
			get
			{
				return this._ReceivedInvitation.get_Count();
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
			SubscribeResponse.Deserialize(stream, this);
		}

		public static SubscribeResponse Deserialize(Stream stream, SubscribeResponse instance)
		{
			return SubscribeResponse.Deserialize(stream, instance, -1L);
		}

		public static SubscribeResponse DeserializeLengthDelimited(Stream stream)
		{
			SubscribeResponse subscribeResponse = new SubscribeResponse();
			SubscribeResponse.DeserializeLengthDelimited(stream, subscribeResponse);
			return subscribeResponse;
		}

		public static SubscribeResponse DeserializeLengthDelimited(Stream stream, SubscribeResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return SubscribeResponse.Deserialize(stream, instance, num);
		}

		public static SubscribeResponse Deserialize(Stream stream, SubscribeResponse instance, long limit)
		{
			if (instance.Collection == null)
			{
				instance.Collection = new List<InvitationCollection>();
			}
			if (instance.ReceivedInvitation == null)
			{
				instance.ReceivedInvitation = new List<Invitation>();
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
							instance.ReceivedInvitation.Add(Invitation.DeserializeLengthDelimited(stream));
						}
					}
					else
					{
						instance.Collection.Add(InvitationCollection.DeserializeLengthDelimited(stream));
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
			SubscribeResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, SubscribeResponse instance)
		{
			if (instance.Collection.get_Count() > 0)
			{
				using (List<InvitationCollection>.Enumerator enumerator = instance.Collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InvitationCollection current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						InvitationCollection.Serialize(stream, current);
					}
				}
			}
			if (instance.ReceivedInvitation.get_Count() > 0)
			{
				using (List<Invitation>.Enumerator enumerator2 = instance.ReceivedInvitation.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Invitation current2 = enumerator2.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current2.GetSerializedSize());
						Invitation.Serialize(stream, current2);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Collection.get_Count() > 0)
			{
				using (List<InvitationCollection>.Enumerator enumerator = this.Collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InvitationCollection current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.ReceivedInvitation.get_Count() > 0)
			{
				using (List<Invitation>.Enumerator enumerator2 = this.ReceivedInvitation.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Invitation current2 = enumerator2.get_Current();
						num += 1u;
						uint serializedSize2 = current2.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			return num;
		}

		public void AddCollection(InvitationCollection val)
		{
			this._Collection.Add(val);
		}

		public void ClearCollection()
		{
			this._Collection.Clear();
		}

		public void SetCollection(List<InvitationCollection> val)
		{
			this.Collection = val;
		}

		public void AddReceivedInvitation(Invitation val)
		{
			this._ReceivedInvitation.Add(val);
		}

		public void ClearReceivedInvitation()
		{
			this._ReceivedInvitation.Clear();
		}

		public void SetReceivedInvitation(List<Invitation> val)
		{
			this.ReceivedInvitation = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<InvitationCollection>.Enumerator enumerator = this.Collection.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InvitationCollection current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			using (List<Invitation>.Enumerator enumerator2 = this.ReceivedInvitation.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Invitation current2 = enumerator2.get_Current();
					num ^= current2.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			SubscribeResponse subscribeResponse = obj as SubscribeResponse;
			if (subscribeResponse == null)
			{
				return false;
			}
			if (this.Collection.get_Count() != subscribeResponse.Collection.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Collection.get_Count(); i++)
			{
				if (!this.Collection.get_Item(i).Equals(subscribeResponse.Collection.get_Item(i)))
				{
					return false;
				}
			}
			if (this.ReceivedInvitation.get_Count() != subscribeResponse.ReceivedInvitation.get_Count())
			{
				return false;
			}
			for (int j = 0; j < this.ReceivedInvitation.get_Count(); j++)
			{
				if (!this.ReceivedInvitation.get_Item(j).Equals(subscribeResponse.ReceivedInvitation.get_Item(j)))
				{
					return false;
				}
			}
			return true;
		}

		public static SubscribeResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<SubscribeResponse>(bs, 0, -1);
		}
	}
}
