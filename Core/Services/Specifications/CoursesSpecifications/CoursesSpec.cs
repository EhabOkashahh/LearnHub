using Domain.Entities.Courses;
using Domain.Entities.Courses.Enums;
using Shared.DTOS.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class CoursesSpec : Specifications<Guid,Course>
    {
        public CoursesSpec() : base(null)
        {
            ApplyIncludeExpression();
        }
        public CoursesSpec(Guid id) : base(C => C.Id == id )
        {
            
        }
        public CoursesSpec(Guid id,string userId) : base(C => C.Id == id && C.Instructor.Id == userId)
        {
            ApplyIncludeExpression();
        }


        public CoursesSpec(CourseQueryParams queryParams)
        : base(C => 
            (!queryParams.Level.HasValue || C.Level == queryParams.Level) 
            &&
            (!queryParams.CategpryId.HasValue || C.CategoryId == queryParams.CategpryId)
            &&
            (string.IsNullOrEmpty(queryParams.search) || C.Title.ToLower().Contains(queryParams.search.ToLower()) || C.Description!.ToLower().Contains(queryParams.search.ToLower())))
        {
            

            ApplyPagination(queryParams.PageIndex, queryParams.PageSize);
            ApplySorting(queryParams.sort);
            ApplyIncludeExpression();
        }



        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        AddOrderByAsc(C => C.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDesc(C => C.Price);
                        break;
                    case "leveldesc":
                        AddOrderByDesc(C => C.Level);
                        break;
                    case "levelasc":
                        AddOrderByAsc(C => C.Level);
                        break;
                    default:
                        AddOrderByAsc(C => C.TotalDurationMinutes);
                        break;
                }
            }
            else
            {
                AddOrderByAsc(C => C.TotalDurationMinutes);
            }
        }
        private void ApplyIncludeExpression()
        {
            IncludeExpression.Add(X => X.Category);
            IncludeExpression.Add(X => X.CourseSections);
        }
    }
}