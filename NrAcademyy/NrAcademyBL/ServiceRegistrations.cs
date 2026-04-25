
using AutoMapper;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using NrAcademyBL.Configuration;
using NrAcademyBL.Services.Abstract;
using NrAcademyBL.Services.Concrete;
using NrAcademyDAL;

namespace NrAcademyBL
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
   
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ITestResultService, TestResultService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ICourseService, CourseService>();


            return services;
        }
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceRegistrations));
            return services;
        }


    }
}
