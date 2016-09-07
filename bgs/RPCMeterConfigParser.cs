using bnet.protocol.config;
using System;
using System.Collections.Generic;

namespace bgs
{
	public class RPCMeterConfigParser
	{
		public static RPCMethodConfig ParseMethod(Tokenizer tokenizer)
		{
			RPCMethodConfig rPCMethodConfig = new RPCMethodConfig();
			tokenizer.NextOpenBracket();
			while (true)
			{
				string text = tokenizer.NextString();
				if (text == null)
				{
					break;
				}
				if (text == "}")
				{
					return rPCMethodConfig;
				}
				string text2 = text;
				if (text2 == null)
				{
					goto IL_1E6;
				}
				if (RPCMeterConfigParser.<>f__switch$mapC == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(11);
					dictionary.Add("service_name:", 0);
					dictionary.Add("method_name:", 1);
					dictionary.Add("fixed_call_cost:", 2);
					dictionary.Add("fixed_packet_size:", 3);
					dictionary.Add("variable_multiplier:", 4);
					dictionary.Add("multiplier:", 5);
					dictionary.Add("rate_limit_count:", 6);
					dictionary.Add("rate_limit_seconds:", 7);
					dictionary.Add("max_packet_size:", 8);
					dictionary.Add("max_encoded_size:", 9);
					dictionary.Add("timeout:", 10);
					RPCMeterConfigParser.<>f__switch$mapC = dictionary;
				}
				int num;
				if (!RPCMeterConfigParser.<>f__switch$mapC.TryGetValue(text2, ref num))
				{
					goto IL_1E6;
				}
				switch (num)
				{
				case 0:
					rPCMethodConfig.ServiceName = tokenizer.NextQuotedString();
					break;
				case 1:
					rPCMethodConfig.MethodName = tokenizer.NextQuotedString();
					break;
				case 2:
					rPCMethodConfig.FixedCallCost = tokenizer.NextUInt32();
					break;
				case 3:
					rPCMethodConfig.FixedPacketSize = tokenizer.NextUInt32();
					break;
				case 4:
					rPCMethodConfig.VariableMultiplier = tokenizer.NextUInt32();
					break;
				case 5:
					rPCMethodConfig.Multiplier = tokenizer.NextFloat();
					break;
				case 6:
					rPCMethodConfig.RateLimitCount = tokenizer.NextUInt32();
					break;
				case 7:
					rPCMethodConfig.RateLimitSeconds = tokenizer.NextUInt32();
					break;
				case 8:
					rPCMethodConfig.MaxPacketSize = tokenizer.NextUInt32();
					break;
				case 9:
					rPCMethodConfig.MaxEncodedSize = tokenizer.NextUInt32();
					break;
				case 10:
					rPCMethodConfig.Timeout = tokenizer.NextFloat();
					break;
				default:
					goto IL_1E6;
				}
				continue;
				IL_1E6:
				tokenizer.SkipUnknownToken();
			}
			throw new Exception("Parsing ended with unfinished RPCMethodConfig");
		}

		public static RPCMeterConfig ParseConfig(string str)
		{
			RPCMeterConfig rPCMeterConfig = new RPCMeterConfig();
			Tokenizer tokenizer = new Tokenizer(str);
			while (true)
			{
				string text = tokenizer.NextString();
				if (text == null)
				{
					break;
				}
				string text2 = text;
				if (text2 == null)
				{
					goto IL_108;
				}
				if (RPCMeterConfigParser.<>f__switch$mapD == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("method", 0);
					dictionary.Add("income_per_second:", 1);
					dictionary.Add("initial_balance:", 2);
					dictionary.Add("cap_balance:", 3);
					dictionary.Add("startup_period:", 4);
					RPCMeterConfigParser.<>f__switch$mapD = dictionary;
				}
				int num;
				if (!RPCMeterConfigParser.<>f__switch$mapD.TryGetValue(text2, ref num))
				{
					goto IL_108;
				}
				switch (num)
				{
				case 0:
					rPCMeterConfig.AddMethod(RPCMeterConfigParser.ParseMethod(tokenizer));
					break;
				case 1:
					rPCMeterConfig.IncomePerSecond = tokenizer.NextUInt32();
					break;
				case 2:
					rPCMeterConfig.InitialBalance = tokenizer.NextUInt32();
					break;
				case 3:
					rPCMeterConfig.CapBalance = tokenizer.NextUInt32();
					break;
				case 4:
					rPCMeterConfig.StartupPeriod = tokenizer.NextFloat();
					break;
				default:
					goto IL_108;
				}
				continue;
				IL_108:
				tokenizer.SkipUnknownToken();
			}
			return rPCMeterConfig;
		}
	}
}
