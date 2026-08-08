using Microsoft.Extensions.DependencyInjection;
using Services.Auth;
using Services.Cache;
using Services.Cart;
using Services.Courses;
using Services.Mapping;
using Services.Users;
using ServicesAbstraction;
using ServicesAbstraction.Auth;
using ServicesAbstraction.Cart;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;
using ServicesAbstraction.Users;

namespace Services
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection ApplyApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICoursesService, CourseService>();
            services.AddScoped<ICourseSectionsService, CourseSectionsService>();
            services.AddScoped<ILessonsService, LessonsService>();
            services.AddScoped<ICategoriesService, CategoryService>();
            services.AddScoped<IEnrollmentsService, EnrollmentsService>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUsersService, UserService>();
            services.AddScoped<IAdminService, AdminServices>();
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CoursesProfile).Assembly));

            return services;
        }
    }
}
