
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NrAcademyBL;
using NrAcademyBL.Exceptions.AuthException;
using NrAcademyBL.Extensions;
using NrAcademyBL.Services.Abstract.BlogPostAbstract;
using NrAcademyBL.Services.Concrete.BlogPostConcrete;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Repositories;
using NrAcademyDAL;
using NrAcademyDAL.Context;
using NrAcademyDAL.Repositories;
using System.Text;
using ServiceRegistrations = NrAcademyBL.ServiceRegistrations;
namespace NrAcademyy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ServiceRegistrations).Assembly);
            });

            builder.Services.AddRepositories();
            builder.Services.AddService();
            //builder.Services.AddAutoMapper();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])
                    )
                };
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PsychologyApi", Version = "v1" });

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
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            

            builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

            
            builder.Services.AddScoped<IBlogPostService, BlogPostService>();

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
            app.UseUserSeedAsync();

            app.MapControllers();

            app.Run();
        }
    }
}
