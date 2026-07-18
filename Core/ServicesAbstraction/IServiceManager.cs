using ServicesAbstraction.Auth;
using ServicesAbstraction.Cart;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;
using ServicesAbstraction.Users;

namespace ServicesAbstraction
{
    public interface IServiceManager
    {
        public ICoursesService CourseService { get; }
        public ICategoriesService CategoryService { get; }
        public ICartServices CartServices { get; }
        public ICacheService CacheService { get;}
        public IAuthService AuthService { get;}
        public IUsersService UserService { get;}
    }
}