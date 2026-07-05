using AutoMapper;
using Domain.Entities.Courses;
using Shared.DTOS.Categories;

namespace Services.Mapping
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryResponse>()
                .ForMember(D => D.CoursesCount, O => O.MapFrom(S => S.Courses.Count));
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
        }
    }
}
