using bgs;
using System;
using System.Collections.Generic;
using UnityEngine;

internal class MyClientInterface : ClientInterface
{
	public string m_temporaryCachePath = Application.get_persistentDataPath();

	private void SetCachePath(string cachePath)
	{
		this.m_temporaryCachePath = cachePath;
	}

	public string GetVersion()
	{
		return string.Empty;
	}

	public bool IsVersionInt()
	{
		return true;
	}

	public string GetBasePersistentDataPath()
	{
		return this.m_temporaryCachePath;
	}

	public constants.RuntimeEnvironment GetRuntimeEnvironment()
	{
		return constants.RuntimeEnvironment.Mono;
	}

	public string GetUserAgent()
	{
		return null;
	}

	public string GetTemporaryCachePath()
	{
		return this.m_temporaryCachePath;
	}

	public bool GetDisableConnectionMetering()
	{
		return true;
	}

	public constants.MobileEnv GetMobileEnvironment()
	{
		string text = Login.m_portal.ToLower();
		if (text != null)
		{
			if (MyClientInterface.<>f__switch$map6 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
				dictionary.Add("us", 0);
				dictionary.Add("eu", 0);
				dictionary.Add("kr", 0);
				dictionary.Add("cn", 0);
				dictionary.Add("tw", 0);
				dictionary.Add("beta", 0);
				MyClientInterface.<>f__switch$map6 = dictionary;
			}
			int num;
			if (MyClientInterface.<>f__switch$map6.TryGetValue(text, ref num))
			{
				if (num == 0)
				{
					return constants.MobileEnv.PRODUCTION;
				}
			}
		}
		return constants.MobileEnv.DEVELOPMENT;
	}

	public string GetAuroraVersionName()
	{
		return "0";
	}

	public string GetLocaleName()
	{
		return Main.instance.GetLocale();
	}

	public string GetPlatformName()
	{
		return "And";
	}

	public IUrlDownloader GetUrlDownloader()
	{
		return Login.instance.m_urlDownloader;
	}
}
