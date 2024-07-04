namespace RewardTaiwan
{
	public static class Configs
	{
		public static UseGoogleAuthentication UseGoogleAuth { get; set; }
		public static SitePages Pages { get; set; }

		public class UseGoogleAuthentication
		{
			public static string ClientId { get; set; }
			public static string ClientSecret { get; set; }
		}
		public class SitePages
		{
			public static string HomePage { get; set; }
		}
	}
}
