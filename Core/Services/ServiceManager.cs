using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using ServicesAbstraction;
using ServicesAbstraction.Cart;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;

namespace Services
{
    public class ServiceManager(IUnitOfWork _uof, IMapper mapper, ICartRepository cartRepository, IDistributedCache distributedCache) : IServiceManager
    {
        public ICoursesService CourseService { get; } = new CourseService(_uof, mapper);
        public ICategoriesService CategoryService { get; } = new CategoryService(_uof, mapper);
        public ICartServices CartServices { get; } = new CartServices(cartRepository, mapper);
        public ICacheService CacheService { get; } = new CacheService(distributedCache);
    }
}