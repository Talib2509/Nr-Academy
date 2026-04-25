
using Microsoft.Extensions.DependencyInjection;
using NrAcademyBL.Services.Abstract;
using NrAcademyBL.Services.Concrete;
using NrAcademyDAL;
using AutoMapper;

namespace NrAcademyBL
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ITestResultService, TestResultService>();
            return services;
        }
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceRegistrations));
            return services;
        }


    }
}
