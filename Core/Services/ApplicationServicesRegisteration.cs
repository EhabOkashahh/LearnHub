using Microsoft.Extensions.DependencyInjection;
using Services.Mapping;
using ServicesAbstraction;
using ServicesAbstraction.Auth;

namespace Services
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection ApplyApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CoursesProfile).Assembly));

            
            return services;
        }
    }
}