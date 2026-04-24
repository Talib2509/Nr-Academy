
using Microsoft.Extensions.DependencyInjection;
using NrAcademyBL.Services.Abstract;
using NrAcademyBL.Services.Abstract.BlogCategoryAbstract;
using NrAcademyBL.Services.Abstract.CertificateAbstract;
using NrAcademyBL.Services.Abstract.EnrollmentAbstract;
using NrAcademyBL.Services.Concrete;
using NrAcademyBL.Services.Concrete.BlogCategoryConcrete;
using NrAcademyBL.Services.Concrete.CertificateConcrete;
using NrAcademyBL.Services.Concrete.EnrollmentConcrete;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Repositories;


namespace NrAcademyBL
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
           services.AddScoped<IEnrollmentService, EnrollmentService>();

            services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();

            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICertificateService, CertificateService>();


            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            return services;
        }

        //public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        //{
        //    services.AddAutoMapper(typeof(ServiceRegistrations));
        //    return services;
        //}
    }
}
