using Microsoft.Extensions.DependencyInjection;
using NrAcademyBL.Services.Abstract;
using NrAcademyBL.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();


            return services;
        }
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceRegistrations));
            return services;
        }
    }
}
