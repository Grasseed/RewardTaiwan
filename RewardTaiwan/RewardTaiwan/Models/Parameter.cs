namespace RewardTaiwan.Models
{
	public class Parameter
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public Parameter(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}

}
