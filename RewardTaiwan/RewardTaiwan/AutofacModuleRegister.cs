using Autofac;
using RewardTaiwan.Services.Interface;
using RewardTaiwan.Services;

namespace RewardTaiwan
{
	public class AutofacModuleRegister : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			//RegisterAssemblyTypes => 註冊所有集合
			//Where(t => t.Name.EndsWith("Service")) => 找出所有Service結尾的檔案
			//AsImplementedInterfaces => 找到Service後註冊到其所繼承的介面 
			builder.RegisterAssemblyTypes(typeof(Program).Assembly)
				.Where(t => t.Name.EndsWith("Service"))
				.AsImplementedInterfaces();
			// 註冊 DapperService 並提供 connectionString
			builder.Register(c =>
			{
				var configuration = c.Resolve<IConfiguration>();
				var connectionString = configuration.GetConnectionString("DefaultConnection");
				return new DapperService(connectionString);
			}).As<IDapper>().InstancePerLifetimeScope();
		}
	}
}
