using System.Collections.Generic;

namespace StarStealer.Entities
{
	public class Browser
	{
		public List<Profile> Profiles = new List<Profile>();

		public string Name
		{
			get;
			set;
		}

		public string LoginData
		{
			get;
			set;
		}

		public string Folder
		{
			get;
			set;
		}

		public string WebData
		{
			get;
			set;
		}

		public string CookiesData
		{
			get;
			set;
		}
	}
}
