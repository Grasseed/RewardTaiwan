using Autofac;
using Autofac.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using RewardTaiwan;
using RewardTaiwan.Services;
using RewardTaiwan.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Autofac settings
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister()));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NLog settings
builder.Services.AddLogging(logging =>
{
	//清除原本的 logging provider
	logging.ClearProviders();
	//設定 logging 的 minmum level 為 trace
	logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
	//使用 NLog 作為 logging provider
	logging.AddNLog();
});

// Dapper services
// Get SQL Connection String from appsettings.json 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDapper>(s => new DapperService(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();