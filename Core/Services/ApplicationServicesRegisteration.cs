using Microsoft.Extensions.DependencyInjection;
using Services.Mapping;
using ServicesAbstraction;

namespace Services
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection ApplyApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CoursesProfile).Assembly));

            
            return services;
        }
    }
}