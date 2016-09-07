using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SecurePlayerPrefs
{
	public static void SetString(string key, string value, string password)
	{
		DESEncryption dESEncryption = new DESEncryption();
		string text = SecurePlayerPrefs.GenerateMD5(key);
		string text2 = dESEncryption.Encrypt(value, password);
		PlayerPrefs.SetString(text, text2);
	}

	public static string GetString(string key, string password)
	{
		string text = SecurePlayerPrefs.GenerateMD5(key);
		if (PlayerPrefs.HasKey(text))
		{
			DESEncryption dESEncryption = new DESEncryption();
			string @string = PlayerPrefs.GetString(text);
			string result;
			dESEncryption.TryDecrypt(@string, password, out result);
			return result;
		}
		return string.Empty;
	}

	public static void DeleteKey(string key)
	{
		string text = SecurePlayerPrefs.GenerateMD5(key);
		PlayerPrefs.DeleteKey(text);
	}

	public static bool HasKey(string key)
	{
		string text = SecurePlayerPrefs.GenerateMD5(key);
		return PlayerPrefs.HasKey(text);
	}

	private static string GenerateMD5(string text)
	{
		MD5 mD = MD5.Create();
		byte[] bytes = Encoding.get_UTF8().GetBytes(text);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
