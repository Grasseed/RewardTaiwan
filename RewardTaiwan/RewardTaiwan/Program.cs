using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using RewardTaiwan;
using RewardTaiwan.Extensions;
using RewardTaiwan.Services;
using RewardTaiwan.Services.Interface;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Autofac settings
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModuleRegister());
});

// Add services to the container.
builder.Services.AddControllers();

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", corsBuilder =>
        corsBuilder.WithOrigins("http://127.0.0.1:5500") // Adjust this to match the URL of your front-end app
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()); // Include if cookies, authorization headers, etc. are needed
});

// Swagger configuration
builder.Services.AddSwaggerGen(options =>
{
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

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement{
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Generate Swagger documentation based on XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// NLog configuration
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    logging.AddNLog();
});

// Dapper services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDapper>(s => new DapperService(connectionString));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register all Config settings
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

app.UseStaticFiles(); // 這允許你的應用服務靜態文件

// Enable CORS with the defined policy
app.UseCors("AllowSpecificOrigin");

app.UseSession();

app.MapControllers();

// Health check endpoint
app.MapGet("/", () => "RewardTaiwan is running.");

app.Run();
