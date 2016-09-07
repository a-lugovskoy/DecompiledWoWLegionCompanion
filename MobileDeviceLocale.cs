using System;
using System.Collections.Generic;
using UnityEngine;

public class MobileDeviceLocale
{
	public struct ConnectionData
	{
		public string address;

		public int port;

		public string version;

		public string name;

		public int tutorialPort;
	}

	private static Dictionary<string, string> s_languageCodeToLocale;

	private static Dictionary<string, int> s_countryCodeToRegionId;

	static MobileDeviceLocale()
	{
		// Note: this type is marked as 'beforefieldinit'.
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("en", "enUS");
		dictionary.Add("fr", "frFR");
		dictionary.Add("de", "deDE");
		dictionary.Add("ko", "koKR");
		dictionary.Add("ru", "ruRU");
		dictionary.Add("it", "itIT");
		dictionary.Add("pt", "ptBR");
		dictionary.Add("en-AU", "enUS");
		dictionary.Add("en-GB", "enUS");
		dictionary.Add("fr-CA", "frFR");
		dictionary.Add("es-MX", "esMX");
		dictionary.Add("zh-Hans", "zhCN");
		dictionary.Add("zh-Hant", "zhTW");
		dictionary.Add("pt-PT", "ptBR");
		MobileDeviceLocale.s_languageCodeToLocale = dictionary;
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		dictionary2.Add("AD", 2);
		dictionary2.Add("AE", 2);
		dictionary2.Add("AG", 1);
		dictionary2.Add("AL", 2);
		dictionary2.Add("AM", 2);
		dictionary2.Add("AO", 2);
		dictionary2.Add("AR", 1);
		dictionary2.Add("AT", 2);
		dictionary2.Add("AU", 1);
		dictionary2.Add("AZ", 2);
		dictionary2.Add("BA", 2);
		dictionary2.Add("BB", 1);
		dictionary2.Add("BD", 1);
		dictionary2.Add("BE", 2);
		dictionary2.Add("BF", 2);
		dictionary2.Add("BG", 2);
		dictionary2.Add("BH", 2);
		dictionary2.Add("BI", 2);
		dictionary2.Add("BJ", 2);
		dictionary2.Add("BM", 2);
		dictionary2.Add("BN", 1);
		dictionary2.Add("BO", 1);
		dictionary2.Add("BR", 1);
		dictionary2.Add("BS", 1);
		dictionary2.Add("BT", 1);
		dictionary2.Add("BW", 2);
		dictionary2.Add("BY", 2);
		dictionary2.Add("BZ", 1);
		dictionary2.Add("CA", 1);
		dictionary2.Add("CD", 2);
		dictionary2.Add("CF", 2);
		dictionary2.Add("CG", 2);
		dictionary2.Add("CH", 2);
		dictionary2.Add("CI", 2);
		dictionary2.Add("CL", 1);
		dictionary2.Add("CM", 2);
		dictionary2.Add("CN", 3);
		dictionary2.Add("CO", 1);
		dictionary2.Add("CR", 1);
		dictionary2.Add("CU", 1);
		dictionary2.Add("CV", 2);
		dictionary2.Add("CY", 2);
		dictionary2.Add("CZ", 2);
		dictionary2.Add("DE", 2);
		dictionary2.Add("DJ", 2);
		dictionary2.Add("DK", 2);
		dictionary2.Add("DM", 1);
		dictionary2.Add("DO", 1);
		dictionary2.Add("DZ", 2);
		dictionary2.Add("EC", 1);
		dictionary2.Add("EE", 2);
		dictionary2.Add("EG", 2);
		dictionary2.Add("ER", 2);
		dictionary2.Add("ES", 2);
		dictionary2.Add("ET", 2);
		dictionary2.Add("FI", 2);
		dictionary2.Add("FJ", 1);
		dictionary2.Add("FK", 2);
		dictionary2.Add("FO", 2);
		dictionary2.Add("FR", 2);
		dictionary2.Add("GA", 2);
		dictionary2.Add("GB", 2);
		dictionary2.Add("GD", 1);
		dictionary2.Add("GE", 2);
		dictionary2.Add("GL", 2);
		dictionary2.Add("GM", 2);
		dictionary2.Add("GN", 2);
		dictionary2.Add("GQ", 2);
		dictionary2.Add("GR", 2);
		dictionary2.Add("GS", 2);
		dictionary2.Add("GT", 1);
		dictionary2.Add("GW", 2);
		dictionary2.Add("GY", 1);
		dictionary2.Add("HK", 3);
		dictionary2.Add("HN", 1);
		dictionary2.Add("HR", 2);
		dictionary2.Add("HT", 1);
		dictionary2.Add("HU", 2);
		dictionary2.Add("ID", 1);
		dictionary2.Add("IE", 2);
		dictionary2.Add("IL", 2);
		dictionary2.Add("IM", 2);
		dictionary2.Add("IN", 1);
		dictionary2.Add("IQ", 2);
		dictionary2.Add("IR", 2);
		dictionary2.Add("IS", 2);
		dictionary2.Add("IT", 2);
		dictionary2.Add("JM", 1);
		dictionary2.Add("JO", 2);
		dictionary2.Add("JP", 3);
		dictionary2.Add("KE", 2);
		dictionary2.Add("KG", 2);
		dictionary2.Add("KH", 2);
		dictionary2.Add("KI", 1);
		dictionary2.Add("KM", 2);
		dictionary2.Add("KP", 1);
		dictionary2.Add("KR", 3);
		dictionary2.Add("KW", 2);
		dictionary2.Add("KY", 2);
		dictionary2.Add("KZ", 2);
		dictionary2.Add("LA", 1);
		dictionary2.Add("LB", 2);
		dictionary2.Add("LC", 1);
		dictionary2.Add("LI", 2);
		dictionary2.Add("LK", 1);
		dictionary2.Add("LR", 2);
		dictionary2.Add("LS", 2);
		dictionary2.Add("LT", 2);
		dictionary2.Add("LU", 2);
		dictionary2.Add("LV", 2);
		dictionary2.Add("LY", 2);
		dictionary2.Add("MA", 2);
		dictionary2.Add("MC", 2);
		dictionary2.Add("MD", 2);
		dictionary2.Add("ME", 2);
		dictionary2.Add("MG", 2);
		dictionary2.Add("MK", 2);
		dictionary2.Add("ML", 2);
		dictionary2.Add("MM", 1);
		dictionary2.Add("MN", 2);
		dictionary2.Add("MO", 3);
		dictionary2.Add("MR", 2);
		dictionary2.Add("MT", 2);
		dictionary2.Add("MU", 2);
		dictionary2.Add("MV", 2);
		dictionary2.Add("MW", 2);
		dictionary2.Add("MX", 1);
		dictionary2.Add("MY", 1);
		dictionary2.Add("MZ", 2);
		dictionary2.Add("NA", 2);
		dictionary2.Add("NC", 2);
		dictionary2.Add("NE", 2);
		dictionary2.Add("NG", 2);
		dictionary2.Add("NI", 1);
		dictionary2.Add("NL", 2);
		dictionary2.Add("NO", 2);
		dictionary2.Add("NP", 1);
		dictionary2.Add("NR", 1);
		dictionary2.Add("NZ", 1);
		dictionary2.Add("OM", 2);
		dictionary2.Add("PA", 1);
		dictionary2.Add("PE", 1);
		dictionary2.Add("PF", 1);
		dictionary2.Add("PG", 1);
		dictionary2.Add("PH", 1);
		dictionary2.Add("PK", 2);
		dictionary2.Add("PL", 2);
		dictionary2.Add("PT", 2);
		dictionary2.Add("PY", 1);
		dictionary2.Add("QA", 2);
		dictionary2.Add("RO", 2);
		dictionary2.Add("RS", 2);
		dictionary2.Add("RU", 2);
		dictionary2.Add("RW", 2);
		dictionary2.Add("SA", 2);
		dictionary2.Add("SB", 1);
		dictionary2.Add("SC", 2);
		dictionary2.Add("SD", 2);
		dictionary2.Add("SE", 2);
		dictionary2.Add("SG", 1);
		dictionary2.Add("SH", 2);
		dictionary2.Add("SI", 2);
		dictionary2.Add("SK", 2);
		dictionary2.Add("SL", 2);
		dictionary2.Add("SN", 2);
		dictionary2.Add("SO", 2);
		dictionary2.Add("SR", 2);
		dictionary2.Add("ST", 2);
		dictionary2.Add("SV", 1);
		dictionary2.Add("SY", 2);
		dictionary2.Add("SZ", 2);
		dictionary2.Add("TD", 2);
		dictionary2.Add("TG", 2);
		dictionary2.Add("TH", 1);
		dictionary2.Add("TJ", 2);
		dictionary2.Add("TL", 1);
		dictionary2.Add("TM", 2);
		dictionary2.Add("TN", 2);
		dictionary2.Add("TO", 1);
		dictionary2.Add("TR", 2);
		dictionary2.Add("TT", 1);
		dictionary2.Add("TV", 1);
		dictionary2.Add("TW", 3);
		dictionary2.Add("TZ", 2);
		dictionary2.Add("UA", 2);
		dictionary2.Add("UG", 2);
		dictionary2.Add("US", 1);
		dictionary2.Add("UY", 1);
		dictionary2.Add("UZ", 2);
		dictionary2.Add("VA", 2);
		dictionary2.Add("VC", 1);
		dictionary2.Add("VE", 1);
		dictionary2.Add("VN", 1);
		dictionary2.Add("VU", 1);
		dictionary2.Add("WS", 1);
		dictionary2.Add("YE", 2);
		dictionary2.Add("YU", 2);
		dictionary2.Add("ZA", 2);
		dictionary2.Add("ZM", 2);
		dictionary2.Add("ZW", 2);
		MobileDeviceLocale.s_countryCodeToRegionId = dictionary2;
	}

	public static string GetBestGuessForLocale()
	{
		string result = "enUS";
		bool flag = false;
		string text = MobileDeviceLocale.GetLanguageCode();
		try
		{
			flag = MobileDeviceLocale.s_languageCodeToLocale.TryGetValue(text, ref result);
		}
		catch (Exception)
		{
		}
		if (!flag)
		{
			text = text.Substring(0, 2);
			try
			{
				flag = MobileDeviceLocale.s_languageCodeToLocale.TryGetValue(text, ref result);
			}
			catch (Exception)
			{
			}
		}
		if (!flag)
		{
			int num = 1;
			string countryCode = MobileDeviceLocale.GetCountryCode();
			try
			{
				MobileDeviceLocale.s_countryCodeToRegionId.TryGetValue(countryCode, ref num);
			}
			catch (Exception)
			{
			}
			string text2 = text;
			if (text2 != null)
			{
				if (MobileDeviceLocale.<>f__switch$map7 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("es", 0);
					dictionary.Add("zh", 1);
					MobileDeviceLocale.<>f__switch$map7 = dictionary;
				}
				int num2;
				if (MobileDeviceLocale.<>f__switch$map7.TryGetValue(text2, ref num2))
				{
					if (num2 == 0)
					{
						if (num == 1)
						{
							result = "esMX";
						}
						else
						{
							result = "esES";
						}
						return result;
					}
					if (num2 == 1)
					{
						if (countryCode == "CN")
						{
							result = "zhCN";
						}
						else
						{
							result = "zhTW";
						}
						return result;
					}
				}
			}
			result = "enUS";
		}
		return result;
	}

	public static string GetCountryCode()
	{
		return MobileDeviceLocale.GetLocaleCountryCode();
	}

	public static string GetLanguageCode()
	{
		return MobileDeviceLocale.GetLocaleLanguageCode();
	}

	private static string GetLocaleCountryCode()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.blizzard.wowcompanion.DeviceSettings");
		return androidJavaClass.CallStatic<string>("GetLocaleCountryCode", new object[0]);
	}

	private static string GetLocaleLanguageCode()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.blizzard.wowcompanion.DeviceSettings");
		return androidJavaClass.CallStatic<string>("GetLocaleLanguageCode", new object[0]);
	}
}
