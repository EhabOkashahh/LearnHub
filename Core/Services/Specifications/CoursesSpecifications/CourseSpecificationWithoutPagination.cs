using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Courses;
using Shared.DTOS.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class CourseSpecificationWithoutPagination<Tkey, TEntity> : Specifications<Guid, Course>
    {
        public CourseSpecificationWithoutPagination(CourseQueryParams queryParams) : base(C => 
            (!queryParams.Level.HasValue || C.Level == queryParams.Level) 
            &&
            (!queryParams.CategpryId.HasValue || C.CategoryId == queryParams.CategpryId)
            &&
            (string.IsNullOrEmpty(queryParams.search) || C.Title.ToLower().Contains(queryParams.search.ToLower()) || (C.Description ?? "").ToLower().Contains(queryParams.search.ToLower())))
        {
            
        }

        public CourseSpecificationWithoutPagination(string userId) : base(C => C.InstructorId == userId)
        {
        }
    }
}