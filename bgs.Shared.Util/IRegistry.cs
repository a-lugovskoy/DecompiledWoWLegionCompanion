using System;

namespace bgs.Shared.Util
{
	public interface IRegistry
	{
		bgs.BattleNetErrors RetrieveVector(string path, string name, out byte[] vec, bool encrypt = true);

		bgs.BattleNetErrors RetrieveString(string path, string name, out string s, bool encrypt = false);

		bgs.BattleNetErrors RetrieveInt(string path, string name, out int i);

		bgs.BattleNetErrors DeleteData(string path, string name);
	}
}
