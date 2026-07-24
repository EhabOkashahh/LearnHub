using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class EnrollmentsSpec : Specifications<Guid,Enrollment>
    {
        public EnrollmentsSpec(string studentId, Guid courseId) : base(x => x.StudentId == studentId && x.CourseId == courseId)
        {

        }
        public EnrollmentsSpec(string studentId, CourseQueryParams queryParams, bool paginated = true) : base(
            x => x.StudentId == studentId && 
            (!queryParams.Level.HasValue || x.Course.Level == queryParams.Level) 
            &&
            (!queryParams.CategpryId.HasValue || x.Course.CategoryId == queryParams.CategpryId)
            &&
            (string.IsNullOrEmpty(queryParams.search) || x.Course.Title.ToLower().Contains(queryParams.search.ToLower()) || x.Course.Description!.ToLower().Contains(queryParams.search.ToLower())))
        {
            if (paginated)
            {
                ApplyPagination(queryParams.PageIndex, queryParams.PageSize);
                ApplySorting(queryParams.sort);
                IncludeAction.Add(q => q.Include(x => x.Course)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Course)
                    .ThenInclude(x => x.Instructor)
                    .Include(x => x.Course)
                    .ThenInclude(x => x.CourseSections));
            }
        }

        private void ApplySorting(string? sort)
        {
            switch (sort?.ToLower())
            {
                case "priceasc":
                    AddOrderByAsc(x => x.Course.Price);
                    break;
                case "pricedesc":
                    AddOrderByDesc(x => x.Course.Price);
                    break;
                case "leveldesc":
                    AddOrderByDesc(x => x.Course.Level);
                    break;
                case "levelasc":
                    AddOrderByAsc(x => x.Course.Level);
                    break;
                case "durationasc":
                    AddOrderByAsc(x => x.Course.TotalDurationMinutes);
                    break;
                case "durationdesc":
                    AddOrderByDesc(x => x.Course.TotalDurationMinutes);
                    break;
                default:
                    AddOrderByAsc(x => x.CreatedAt);
                    break;
            }
        }
    }
}