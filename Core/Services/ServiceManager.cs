using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using ServicesAbstraction.Auth;
using ServicesAbstraction.Cart;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;
using ServicesAbstraction.Users;

namespace Services
{
    public class ServiceManager(
        IUnitOfWork _uof,
         IMapper mapper,
         ICartRepository cartRepository,
         IDistributedCache distributedCache,
         UserManager<AppUser> _userManager,
          IOptions<JwtOptions> _jwtOptions,
          IAuthService _auth) : IServiceManager
    {
        public ICoursesService CourseService { get; } = new CourseService(_uof, mapper);
        public ICategoriesService CategoryService { get; } = new CategoryService(_uof, mapper);
        public ICartServices CartServices { get; } = new CartServices(cartRepository, mapper);
        public ICacheService CacheService { get; } = new CacheService(distributedCache);
        public IAuthService AuthService { get; } = new AuthService(_userManager, mapper, _jwtOptions);
        public IUsersService UserService { get; } = new UserService(_userManager, mapper, _uof, _auth);
        public IAdminService AdminService { get; } = new AdminServices(_uof, mapper, _userManager);
    }
}