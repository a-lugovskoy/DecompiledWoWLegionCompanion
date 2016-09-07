using bgs.Shared.Util.platform_android;
using System;

namespace bgs.Shared.Util
{
	public class Registry
	{
		private static readonly IRegistry mImpl = new RegistryAndroid();

		public static readonly byte[] s_entropy = new byte[]
		{
			200,
			118,
			244,
			174,
			76,
			149,
			46,
			254,
			242,
			250,
			15,
			84,
			25,
			192,
			156,
			67
		};

		public static bgs.BattleNetErrors RetrieveVector(string path, string name, out byte[] vec, bool encrypt = true)
		{
			return Registry.mImpl.RetrieveVector(path, name, out vec, encrypt);
		}

		public static bgs.BattleNetErrors RetrieveString(string path, string name, out string s, bool encrypt = false)
		{
			return Registry.mImpl.RetrieveString(path, name, out s, encrypt);
		}

		public static bgs.BattleNetErrors RetrieveInt(string path, string name, out int i)
		{
			return Registry.mImpl.RetrieveInt(path, name, out i);
		}

		private static bgs.BattleNetErrors DeleteData(string path, string name)
		{
			return Registry.mImpl.DeleteData(path, name);
		}
	}
}
