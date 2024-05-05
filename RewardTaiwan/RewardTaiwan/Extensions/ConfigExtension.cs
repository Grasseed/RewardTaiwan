namespace RewardTaiwan.Extensions
{
	public static class ConfigExtension
	{
		public static void ConnInfoSetting(this IServiceCollection services, IConfiguration config)
		{
			//var dbConnections = config.GetSection("DBConnections").Get<List<ConnInfo>>();
			
		}
		public static void ConfigSetting(this IServiceCollection services, IConfiguration config)
		{
			//Configs.Aes = config.GetSection("Aes").Get<Configs.AesSetting>();
			//Configs.Reply = config.GetSection("ReplySetting").Get<Configs.ReplySetting>();
			//Configs.Skype = config.GetSection("SkypeConfig").Get<Configs.SkypeConfig>();
			Configs.UseGoogleAuth = config.GetSection("UseGoogleAuthentication").Get<Configs.UseGoogleAuthentication>();
			Configs.Pages = config.GetSection("SitePages").Get<Configs.SitePages>();
		}
	}
}
