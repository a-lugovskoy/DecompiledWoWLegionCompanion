using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.config
{
	public class RPCMeterConfig : IProtoBuf
	{
		private List<RPCMethodConfig> _Method = new List<RPCMethodConfig>();

		public bool HasIncomePerSecond;

		private uint _IncomePerSecond;

		public bool HasInitialBalance;

		private uint _InitialBalance;

		public bool HasCapBalance;

		private uint _CapBalance;

		public bool HasStartupPeriod;

		private float _StartupPeriod;

		public List<RPCMethodConfig> Method
		{
			get
			{
				return this._Method;
			}
			set
			{
				this._Method = value;
			}
		}

		public List<RPCMethodConfig> MethodList
		{
			get
			{
				return this._Method;
			}
		}

		public int MethodCount
		{
			get
			{
				return this._Method.get_Count();
			}
		}

		public uint IncomePerSecond
		{
			get
			{
				return this._IncomePerSecond;
			}
			set
			{
				this._IncomePerSecond = value;
				this.HasIncomePerSecond = true;
			}
		}

		public uint InitialBalance
		{
			get
			{
				return this._InitialBalance;
			}
			set
			{
				this._InitialBalance = value;
				this.HasInitialBalance = true;
			}
		}

		public uint CapBalance
		{
			get
			{
				return this._CapBalance;
			}
			set
			{
				this._CapBalance = value;
				this.HasCapBalance = true;
			}
		}

		public float StartupPeriod
		{
			get
			{
				return this._StartupPeriod;
			}
			set
			{
				this._StartupPeriod = value;
				this.HasStartupPeriod = true;
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
			RPCMeterConfig.Deserialize(stream, this);
		}

		public static RPCMeterConfig Deserialize(Stream stream, RPCMeterConfig instance)
		{
			return RPCMeterConfig.Deserialize(stream, instance, -1L);
		}

		public static RPCMeterConfig DeserializeLengthDelimited(Stream stream)
		{
			RPCMeterConfig rPCMeterConfig = new RPCMeterConfig();
			RPCMeterConfig.DeserializeLengthDelimited(stream, rPCMeterConfig);
			return rPCMeterConfig;
		}

		public static RPCMeterConfig DeserializeLengthDelimited(Stream stream, RPCMeterConfig instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return RPCMeterConfig.Deserialize(stream, instance, num);
		}

		public static RPCMeterConfig Deserialize(Stream stream, RPCMeterConfig instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (instance.Method == null)
			{
				instance.Method = new List<RPCMethodConfig>();
			}
			instance.IncomePerSecond = 1u;
			instance.StartupPeriod = 0f;
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
						if (num2 != 16)
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
										instance.StartupPeriod = binaryReader.ReadSingle();
									}
								}
								else
								{
									instance.CapBalance = ProtocolParser.ReadUInt32(stream);
								}
							}
							else
							{
								instance.InitialBalance = ProtocolParser.ReadUInt32(stream);
							}
						}
						else
						{
							instance.IncomePerSecond = ProtocolParser.ReadUInt32(stream);
						}
					}
					else
					{
						instance.Method.Add(RPCMethodConfig.DeserializeLengthDelimited(stream));
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
			RPCMeterConfig.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, RPCMeterConfig instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.Method.get_Count() > 0)
			{
				using (List<RPCMethodConfig>.Enumerator enumerator = instance.Method.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RPCMethodConfig current = enumerator.get_Current();
						stream.WriteByte(10);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						RPCMethodConfig.Serialize(stream, current);
					}
				}
			}
			if (instance.HasIncomePerSecond)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, instance.IncomePerSecond);
			}
			if (instance.HasInitialBalance)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt32(stream, instance.InitialBalance);
			}
			if (instance.HasCapBalance)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteUInt32(stream, instance.CapBalance);
			}
			if (instance.HasStartupPeriod)
			{
				stream.WriteByte(45);
				binaryWriter.Write(instance.StartupPeriod);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.Method.get_Count() > 0)
			{
				using (List<RPCMethodConfig>.Enumerator enumerator = this.Method.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RPCMethodConfig current = enumerator.get_Current();
						num += 1u;
						uint serializedSize = current.GetSerializedSize();
						num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
					}
				}
			}
			if (this.HasIncomePerSecond)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.IncomePerSecond);
			}
			if (this.HasInitialBalance)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.InitialBalance);
			}
			if (this.HasCapBalance)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.CapBalance);
			}
			if (this.HasStartupPeriod)
			{
				num += 1u;
				num += 4u;
			}
			return num;
		}

		public void AddMethod(RPCMethodConfig val)
		{
			this._Method.Add(val);
		}

		public void ClearMethod()
		{
			this._Method.Clear();
		}

		public void SetMethod(List<RPCMethodConfig> val)
		{
			this.Method = val;
		}

		public void SetIncomePerSecond(uint val)
		{
			this.IncomePerSecond = val;
		}

		public void SetInitialBalance(uint val)
		{
			this.InitialBalance = val;
		}

		public void SetCapBalance(uint val)
		{
			this.CapBalance = val;
		}

		public void SetStartupPeriod(float val)
		{
			this.StartupPeriod = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			using (List<RPCMethodConfig>.Enumerator enumerator = this.Method.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RPCMethodConfig current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			if (this.HasIncomePerSecond)
			{
				num ^= this.IncomePerSecond.GetHashCode();
			}
			if (this.HasInitialBalance)
			{
				num ^= this.InitialBalance.GetHashCode();
			}
			if (this.HasCapBalance)
			{
				num ^= this.CapBalance.GetHashCode();
			}
			if (this.HasStartupPeriod)
			{
				num ^= this.StartupPeriod.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			RPCMeterConfig rPCMeterConfig = obj as RPCMeterConfig;
			if (rPCMeterConfig == null)
			{
				return false;
			}
			if (this.Method.get_Count() != rPCMeterConfig.Method.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Method.get_Count(); i++)
			{
				if (!this.Method.get_Item(i).Equals(rPCMeterConfig.Method.get_Item(i)))
				{
					return false;
				}
			}
			return this.HasIncomePerSecond == rPCMeterConfig.HasIncomePerSecond && (!this.HasIncomePerSecond || this.IncomePerSecond.Equals(rPCMeterConfig.IncomePerSecond)) && this.HasInitialBalance == rPCMeterConfig.HasInitialBalance && (!this.HasInitialBalance || this.InitialBalance.Equals(rPCMeterConfig.InitialBalance)) && this.HasCapBalance == rPCMeterConfig.HasCapBalance && (!this.HasCapBalance || this.CapBalance.Equals(rPCMeterConfig.CapBalance)) && this.HasStartupPeriod == rPCMeterConfig.HasStartupPeriod && (!this.HasStartupPeriod || this.StartupPeriod.Equals(rPCMeterConfig.StartupPeriod));
		}

		public static RPCMeterConfig ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<RPCMeterConfig>(bs, 0, -1);
		}
	}
}
