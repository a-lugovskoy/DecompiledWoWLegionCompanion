using System;
using System.Collections.Generic;

internal class VarsInternal
{
	private static VarsInternal s_instance = new VarsInternal();

	private Dictionary<string, string> m_vars = new Dictionary<string, string>();

	private VarsInternal()
	{
		string clientConfigPath = Vars.GetClientConfigPath();
		if (!this.LoadConfig(clientConfigPath))
		{
		}
	}

	public static VarsInternal Get()
	{
		return VarsInternal.s_instance;
	}

	public static void RefreshVars()
	{
		VarsInternal.s_instance = new VarsInternal();
	}

	public bool Contains(string key)
	{
		return this.m_vars.ContainsKey(key);
	}

	public string Value(string key)
	{
		return this.m_vars.get_Item(key);
	}

	private bool LoadConfig(string path)
	{
		ConfigFile configFile = new ConfigFile();
		if (!configFile.LightLoad(path))
		{
			return false;
		}
		using (List<ConfigFile.Line>.Enumerator enumerator = configFile.GetLines().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ConfigFile.Line current = enumerator.get_Current();
				this.m_vars.set_Item(current.m_fullKey, current.m_value);
			}
		}
		return true;
	}
}
