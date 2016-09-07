using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class DESEncryption : IEncryption
{
	private const int Iterations = 1000;

	public string Encrypt(string plainText, string password)
	{
		if (plainText == null)
		{
			throw new ArgumentNullException("plainText");
		}
		if (string.IsNullOrEmpty(password))
		{
			throw new ArgumentNullException("password");
		}
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		dESCryptoServiceProvider.GenerateIV();
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, dESCryptoServiceProvider.get_IV(), 1000);
		byte[] bytes = rfc2898DeriveBytes.GetBytes(8);
		string result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, dESCryptoServiceProvider.get_IV()), 1))
			{
				memoryStream.Write(dESCryptoServiceProvider.get_IV(), 0, dESCryptoServiceProvider.get_IV().Length);
				byte[] bytes2 = Encoding.get_UTF8().GetBytes(plainText);
				cryptoStream.Write(bytes2, 0, bytes2.Length);
				cryptoStream.FlushFinalBlock();
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
		}
		return result;
	}

	public bool TryDecrypt(string cipherText, string password, out string plainText)
	{
		if (string.IsNullOrEmpty(cipherText) || string.IsNullOrEmpty(password))
		{
			plainText = string.Empty;
			return false;
		}
		bool result;
		try
		{
			byte[] array = Convert.FromBase64String(cipherText);
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				byte[] array2 = new byte[8];
				memoryStream.Read(array2, 0, array2.Length);
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array2, 1000);
				byte[] bytes = rfc2898DeriveBytes.GetBytes(8);
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, array2), 0))
				{
					using (StreamReader streamReader = new StreamReader(cryptoStream))
					{
						plainText = streamReader.ReadToEnd();
						result = true;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			plainText = string.Empty;
			result = false;
		}
		return result;
	}
}
