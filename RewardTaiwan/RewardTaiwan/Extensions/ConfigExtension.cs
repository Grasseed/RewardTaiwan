namespace RewardTaiwan.Extensions
{
	public static class ConfigExtension
	{
		public static void ConnInfoSetting(this IServiceCollection services, IConfiguration config)
		{
			
		}
		public static void ConfigSetting(this IServiceCollection services, IConfiguration config)
		{
			//Configs.Aes = config.GetSection("Aes").Get<Configs.AesSetting>();
			Configs.UseGoogleAuth = config.GetSection("UseGoogleAuthentication").Get<Configs.UseGoogleAuthentication>();
			Configs.Pages = config.GetSection("SitePages").Get<Configs.SitePages>();
		}
	}
}
