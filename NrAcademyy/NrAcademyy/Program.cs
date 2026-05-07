using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NrAcademyBL;
using NrAcademyBL.Configuration;
using NrAcademyBL.Exceptions;
using NrAcademyBL.Exceptions.AuthException;
using NrAcademyBL.Extensions;
using NrAcademyCORE.Entities.Identity;
using NrAcademyDAL.Context;
using Serilog;
using System.Text;
using NrAcademyDAL; // Və ya metod hansı namespace daxilindədirsə o
// 1. .env faylını yükləyirik
Env.Load();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/nracademy-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("NrAcademy tətbiqi başladılır...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Configuration.AddEnvironmentVariables();
    builder.Services.AddMemoryCache();
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddRepositories();
    builder.Services.AddService(builder.Configuration);

    // Email Settings konfiqurasiyası
    builder.Services.Configure<EmailSettings>(options => {
        // Əgər .env-dən oxuya bilməsə, appsettings-dəki stringi istifadə etməsin deyə default dəyərlər qoyuruq
        options.Host = builder.Configuration[builder.Configuration["EmailSettings:Host"] ?? ""] ?? "smtp.gmail.com";
        options.Port = int.Parse(builder.Configuration["EmailSettings:Port"] ?? "587");
        options.FromEmail = builder.Configuration[builder.Configuration["EmailSettings:FromEmail"] ?? ""] ?? "huseynovmirtalib28@gmail.com";
        options.FromName = builder.Configuration["EmailSettings:FromName"] ?? "NR Academy";
        options.Username = builder.Configuration[builder.Configuration["EmailSettings:Username"] ?? ""] ?? "huseynovmirtalib28@gmail.com";
        options.Password = builder.Configuration[builder.Configuration["EmailSettings:Password"] ?? ""] ?? "";
    });

    // JWT Settings konfiqurasiyası
    builder.Services.Configure<JwtSettings>(options => {
        options.Secret = builder.Configuration[builder.Configuration["JwtSettings:Secret"]];
        options.Issuer = builder.Configuration[builder.Configuration["JwtSettings:Issuer"]];
        options.Audience = builder.Configuration[builder.Configuration["JwtSettings:Audience"]];
        options.AccessTokenMinutes = int.Parse(builder.Configuration["JwtSettings:AccessTokenMinutes"] ?? "15");
        options.RefreshTokenDays = int.Parse(builder.Configuration["JwtSettings:RefreshTokenDays"] ?? "7");
    });

    builder.Services.AddAutoMapper();

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionKey = builder.Configuration.GetConnectionString("DefaultConnection");
        var realConnectionString = builder.Configuration[connectionKey] ?? connectionKey;
        options.UseSqlServer(realConnectionString);
    });

    builder.Services.AddIdentity<AppUser, AppRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<CustomErrorDescriber>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["JwtSettings:Secret"];
        var realSecret = builder.Configuration[secretKey] ?? secretKey;

        var issuerKey = builder.Configuration["JwtSettings:Issuer"];
        var realIssuer = builder.Configuration[issuerKey] ?? issuerKey;

        var audienceKey = builder.Configuration["JwtSettings:Audience"];
        var realAudience = builder.Configuration[audienceKey] ?? audienceKey;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = realIssuer,
            ValidAudience = realAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(realSecret))
        };
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "NrAcademyApi", Version = "v1" });

        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Tokeni daxil edin",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[]{}
            }
        });
    });

    // 2. Build əmri burada olmalıdır
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseUserSeedAsync();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Tətbiq xəta verdi");
}
finally
{
    Log.CloseAndFlush();
}