using System;

namespace bgs.Shared.Util.platform_android
{
	public class RegistryAndroid : IRegistry
	{
		public bgs.BattleNetErrors RetrieveVector(string path, string name, out byte[] vec, bool encrypt = true)
		{
			vec = null;
			return bgs.BattleNetErrors.ERROR_REGISTRY_NOT_FOUND;
		}

		public bgs.BattleNetErrors RetrieveString(string path, string name, out string s, bool encrypt = false)
		{
			s = string.Empty;
			return bgs.BattleNetErrors.ERROR_REGISTRY_NOT_FOUND;
		}

		public bgs.BattleNetErrors RetrieveInt(string path, string name, out int i)
		{
			i = 0;
			return bgs.BattleNetErrors.ERROR_REGISTRY_NOT_FOUND;
		}

		public bgs.BattleNetErrors DeleteData(string path, string name)
		{
			return bgs.BattleNetErrors.ERROR_REGISTRY_DELETE_ERROR;
		}
	}
}
