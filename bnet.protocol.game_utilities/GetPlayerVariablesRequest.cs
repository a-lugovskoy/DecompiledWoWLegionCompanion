using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.game_utilities
{
	public class GetPlayerVariablesRequest : IProtoBuf
	{
		private List<PlayerVariables> _PlayerVariables = new List<PlayerVariables>();

		public bool HasHost;

		private ProcessId _Host;

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

		public ProcessId Host
		{
			get
			{
				return this._Host;
			}
			set
			{
				this._Host = value;
				this.HasHost = (value != null);
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
			GetPlayerVariablesRequest.Deserialize(stream, this);
		}

		public static GetPlayerVariablesRequest Deserialize(Stream stream, GetPlayerVariablesRequest instance)
		{
			return GetPlayerVariablesRequest.Deserialize(stream, instance, -1L);
		}

		public static GetPlayerVariablesRequest DeserializeLengthDelimited(Stream stream)
		{
			GetPlayerVariablesRequest getPlayerVariablesRequest = new GetPlayerVariablesRequest();
			GetPlayerVariablesRequest.DeserializeLengthDelimited(stream, getPlayerVariablesRequest);
			return getPlayerVariablesRequest;
		}

		public static GetPlayerVariablesRequest DeserializeLengthDelimited(Stream stream, GetPlayerVariablesRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GetPlayerVariablesRequest.Deserialize(stream, instance, num);
		}

		public static GetPlayerVariablesRequest Deserialize(Stream stream, GetPlayerVariablesRequest instance, long limit)
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
						else if (instance.Host == null)
						{
							instance.Host = ProcessId.DeserializeLengthDelimited(stream);
						}
						else
						{
							ProcessId.DeserializeLengthDelimited(stream, instance.Host);
						}
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
			GetPlayerVariablesRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GetPlayerVariablesRequest instance)
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
			if (instance.HasHost)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteUInt32(stream, instance.Host.GetSerializedSize());
				ProcessId.Serialize(stream, instance.Host);
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
			if (this.HasHost)
			{
				num += 1u;
				uint serializedSize2 = this.Host.GetSerializedSize();
				num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
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

		public void SetHost(ProcessId val)
		{
			this.Host = val;
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
			if (this.HasHost)
			{
				num ^= this.Host.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			GetPlayerVariablesRequest getPlayerVariablesRequest = obj as GetPlayerVariablesRequest;
			if (getPlayerVariablesRequest == null)
			{
				return false;
			}
			if (this.PlayerVariables.get_Count() != getPlayerVariablesRequest.PlayerVariables.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.PlayerVariables.get_Count(); i++)
			{
				if (!this.PlayerVariables.get_Item(i).Equals(getPlayerVariablesRequest.PlayerVariables.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasHost == getPlayerVariablesRequest.HasHost && (!this.HasHost || this.Host.Equals(getPlayerVariablesRequest.Host));
		}

		public static GetPlayerVariablesRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GetPlayerVariablesRequest>(bs, 0, -1);
		}
	}
}
