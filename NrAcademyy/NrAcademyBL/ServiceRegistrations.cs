using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NrAcademyBL.Services.Abstract;
using NrAcademyBL.Services.Concrete;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Configuration;

namespace NrAcademyBL
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
            // Identity & Security
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            // Core Business Services
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();

            // Test & Exam System
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<ITestResultService, TestResultService>();
            services.AddScoped<ICertificateService, CertificateService>();

            // Infrastructure
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            // Profil klasslarının olduğu assembly-ni avtomatik skan edir
            services.AddAutoMapper(typeof(ServiceRegistrations).Assembly);
            return services;
        }
    }
}