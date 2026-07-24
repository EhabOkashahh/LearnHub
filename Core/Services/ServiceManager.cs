using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicesAbstraction;
using ServicesAbstraction.Auth;
using ServicesAbstraction.Cart;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;
using ServicesAbstraction.Users;

namespace Services
{
    public class ServiceManager(
        ICoursesService courseService,
        ICourseSectionsService courseSectionsService,
        ILessonsService lessonsService,
        ICategoriesService categoryService,
        ICartServices cartServices,
        ICacheService cacheService,
        IAuthService authService,
        IUsersService userService,
        IAdminService adminService,
        IEnrollmentsService enrollmentsService) : IServiceManager 
    {
        public ICoursesService CourseService { get; } = courseService;
        public ICourseSectionsService CourseSectionsService { get; } = courseSectionsService;
        public ILessonsService LessonsService { get; } = lessonsService;
        public ICategoriesService CategoryService { get; } = categoryService;
        public ICartServices CartServices { get; } = cartServices;
        public ICacheService CacheService { get; } = cacheService;
        public IAuthService AuthService { get; } = authService;
        public IUsersService UserService { get; } = userService;
        public IAdminService AdminService { get; } = adminService;
        public IEnrollmentsService EnrollmentsService { get; } = enrollmentsService;
    }
}