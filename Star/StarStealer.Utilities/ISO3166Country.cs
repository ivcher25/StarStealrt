namespace StarStealer.Utilities
{
	public class ISO3166Country
	{
		public string Name
		{
			get;
			private set;
		}

		public string Alpha2
		{
			get;
			private set;
		}

		public string Alpha3
		{
			get;
			private set;
		}

		public int NumericCode
		{
			get;
			private set;
		}

		public ISO3166Country(string name, string alpha2, string alpha3, int numericCode)
		{
			Name = name;
			Alpha2 = alpha2;
			Alpha3 = alpha3;
			NumericCode = numericCode;
		}
	}
}
