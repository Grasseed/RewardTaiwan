using Autofac;
using Autofac.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using RewardTaiwan;
using RewardTaiwan.Extensions;
using RewardTaiwan.Services;
using RewardTaiwan.Services.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Autofac settings
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister()));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	// Add Swagger Documentation
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Version = "v1",
		Title = "RewardTaiwan",
		Description = "ASP.NET Core Web API for RewardTaiwan",
	});

	// Authorization
	options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme."
	});

	options.AddSecurityRequirement(
		new Microsoft.OpenApi.Models.OpenApiSecurityRequirement{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});

	// Ū�� XML �ɮײ��� Swagger ����
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});

// NLog settings
builder.Services.AddLogging(logging =>
{
	//�M���쥻�� logging provider
	logging.ClearProviders();
	//�]�w logging �� minmum level �� trace
	logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
	//�ϥ� NLog �@�� logging provider
	logging.AddNLog();
});

// Dapper services
// Get SQL Connection String from appsettings.json 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDapper>(s => new DapperService(connectionString));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// ���U�Ҧ�Config�]�w
builder.Services.ConfigSetting(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.UseSession();

app.MapControllers();

// �ˬd���x�O�_���`�B�@
app.MapGet("/", () => "RewardTaiwan is running.");

app.Run();