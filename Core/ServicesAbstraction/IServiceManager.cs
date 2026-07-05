using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;

namespace ServicesAbstraction
{
    public interface IServiceManager
    {
        public ICoursesService CourseService { get; }
        public ICategoriesService CategoryService { get; }
    }
}