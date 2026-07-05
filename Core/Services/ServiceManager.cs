using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using ServicesAbstraction;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;

namespace Services
{
    public class ServiceManager(IUnitOfWork _uof, IMapper mapper) : IServiceManager
    {
        public ICoursesService CourseService { get; } = new CourseService(_uof, mapper);
        public ICategoriesService CategoryService { get; } = new CategoryService(_uof, mapper);
    }
}