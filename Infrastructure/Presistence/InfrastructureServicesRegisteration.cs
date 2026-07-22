using System.Data.Common;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presistence.Data;
using Presistence.Data.Contexts;
using Presistence.Data.Seeding;
using Presistence.Repository;
using Services;
using ServicesAbstraction;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;

using StackExchange.Redis;

namespace Presistence
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((sp,options) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ICoursesService, CourseService>();
            services.AddScoped<ICourseSectionsService, CourseSectionsService>();
            services.AddScoped<ILessonsService, LessonsService>();
            services.AddScoped<ICategoriesService, CategoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICartRepository,CartRepository>();
            services.AddScoped<ICacheService,CacheService>();
            services.AddScoped<IDbInitializer,DbInitializer>();
            services.AddScoped<IEnrollmentsService, EnrollmentsService>();
            
            services.AddSingleton<IConnectionMultiplexer>((sp) => {
                var config = sp.GetRequiredService<IConfiguration>();
                return ConnectionMultiplexer.Connect(config.GetConnectionString("RedisConnnection")!);
            });

            services.AddStackExchangeRedisCache(op =>
            {
               op.Configuration = configuration.GetConnectionString("RedisConnnection"); 
            });
            return services;
        }
    }
}