using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HC_Odontologicas.FuncionesGenerales
{
	public class Encriptacion
	{
		private const string PASSWORD = "uuttff88";

		public static string Encrypt(string decryptedString)
		{
			string encryptedString = string.Empty;

			byte[] decryptedBytes = UTF8Encoding.UTF8.GetBytes(decryptedString);
			byte[] saltBytes = Encoding.UTF8.GetBytes(PASSWORD);

			using (AesManaged aes = new AesManaged())
			{
				Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(PASSWORD, saltBytes);

				aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
				aes.KeySize = aes.LegalKeySizes[0].MaxSize;
				aes.Key = rfc.GetBytes(Convert.ToInt32(aes.KeySize / 8));
				aes.IV = rfc.GetBytes(Convert.ToInt32(aes.BlockSize / 8));

				using (ICryptoTransform encryptTransform = aes.CreateEncryptor())
				{
					using (MemoryStream encryptedStream = new MemoryStream())
					{
						using (CryptoStream encryptor = new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write))
						{
							encryptor.Write(decryptedBytes, 0, decryptedBytes.Length);
							encryptor.Flush();
							encryptor.Close();

							byte[] encryptedBytes = encryptedStream.ToArray();
							encryptedString = Convert.ToBase64String(encryptedBytes);
						}
					}
				}

			}

			return encryptedString;
		}

		public static string Decrypt(string encryptedString)
		{
			//encryptedString = "+dPM0yb4j4vrnIWnWbxxTg==";
			var primer = encryptedString.Substring(0,1);
			if (primer.Equals(" "))
			{
				encryptedString = "+" + encryptedString;
			}

			string decryptedString = string.Empty;

			byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
			byte[] saltBytes = Encoding.UTF8.GetBytes(PASSWORD);


			using (AesManaged aes = new AesManaged())
			{
				Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(PASSWORD, saltBytes);

				aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
				aes.KeySize = aes.LegalKeySizes[0].MaxSize;
				aes.Key = rfc.GetBytes(Convert.ToInt32(aes.KeySize / 8));
				aes.IV = rfc.GetBytes(Convert.ToInt32(aes.BlockSize / 8));

				using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
				{
					using (MemoryStream decryptedStream = new MemoryStream())
					{
						using (CryptoStream decryptor = new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write))
						{
							decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
							decryptor.Flush();
							decryptor.Close();

							byte[] decryptedBytes = decryptedStream.ToArray();
							decryptedString = UTF8Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
						}
					}
				}

			}
			return decryptedString;
		}
	}
}
