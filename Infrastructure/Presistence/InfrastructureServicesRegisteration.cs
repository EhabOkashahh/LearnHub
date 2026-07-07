using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presistence.Data;
using Presistence.Data.Contexts;
using Services;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;

namespace Presistence
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICoursesService, CourseService>();
            services.AddScoped<ICategoriesService, CategoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}