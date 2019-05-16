using System.Collections.Generic;

namespace StarStealer.Entities
{
	public class Profile
	{
		public List<Password> Passwords = new List<Password>();

		public List<AutoFill> Autofills = new List<AutoFill>();

		public List<CC> Cards = new List<CC>();

		public List<Cookie> Cookies = new List<Cookie>();

		public string Name
		{
			get;
			set;
		}
	}
}
