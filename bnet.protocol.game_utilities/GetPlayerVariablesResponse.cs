using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_utilities
{
	public class GetPlayerVariablesResponse : IProtoBuf
	{
		private List<PlayerVariables> _PlayerVariables = new List<PlayerVariables>();

		public List<PlayerVariables> PlayerVariables
		{
			get
			{
				return this._PlayerVariables;
			}
			set
			{
				this._PlayerVariables = value;
			}
		}

		public List<PlayerVariables> PlayerVariablesList
		{
			get
			{
				return this._PlayerVariables;
			}
		}

		public int PlayerVariablesCount
		{
			get
			{
				return this._PlayerVariables.get_Count();
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
			GetPlayerVariablesResponse.Deserialize(stream, this);
		}

		public static GetPlayerVariablesResponse Deserialize(Stream stream, GetPlayerVariablesResponse instance)
		{
			return GetPlayerVariablesResponse.Deserialize(stream, instance, -1L);
		}

		public static GetPlayerVariablesResponse DeserializeLengthDelimited(Stream stream)
		{
			GetPlayerVariablesResponse getPlayerVariablesResponse = new GetPlayerVariablesResponse();
			GetPlayerVariablesResponse.DeserializeLengthDelimited(stream, getPlayerVariablesResponse);
			return getPlayerVariablesResponse;
		}

		public static GetPlayerVariablesResponse DeserializeLengthDelimited(Stream stream, GetPlayerVariablesResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GetPlayerVariablesResponse.Deserialize(stream, instance, num);
		}

		public static GetPlayerVariablesResponse Deserialize(Stream stream, GetPlayerVariablesResponse instance, long limit)
		{
			if (instance.PlayerVariables == null)
			{
				instance.PlayerVariables = new List<PlayerVariables>();
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
						instance.PlayerVariables.Add(bnet.protocol.game_utilities.PlayerVariables.DeserializeLengthDelimited(stream));
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
			GetPlayerVariablesResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GetPlayerVariablesResponse instance)
		{
			if (instance.PlayerVariables.get_Count() > 0)
			{
				using (List<PlayerVariables>.Enumerator enumerator = instance.PlayerVariables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerVariables current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						bnet.protocol.game_utilities.PlayerVariables.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.PlayerVariables.get_Count() > 0)
			{
				using (List<PlayerVariables>.Enumerator enumerator = this.PlayerVariables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerVariables current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			return num;
		}

		public void AddPlayerVariables(PlayerVariables val)
		{
			this._PlayerVariables.Add(val);
		}

		public void ClearPlayerVariables()
		{
			this._PlayerVariables.Clear();
		}

		public void SetPlayerVariables(List<PlayerVariables> val)
		{
			this.PlayerVariables = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<PlayerVariables>.Enumerator enumerator = this.PlayerVariables.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerVariables current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GetPlayerVariablesResponse getPlayerVariablesResponse = obj as GetPlayerVariablesResponse;
			if (getPlayerVariablesResponse == null)
			{
				return false;
			}
			if (this.PlayerVariables.get_Count() != getPlayerVariablesResponse.PlayerVariables.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.PlayerVariables.get_Count(); i++)
			{
				if (!this.PlayerVariables.get_Item(i).Equals(getPlayerVariablesResponse.PlayerVariables.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static GetPlayerVariablesResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GetPlayerVariablesResponse>(bs, 0, -1);
		}
	}
}
