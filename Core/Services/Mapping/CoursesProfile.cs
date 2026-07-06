using AutoMapper;
using Domain.Entities.Courses;
using Shared.DTOS.Courses;

namespace Services.Mapping
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseResponse>().ForMember(D => D.ThumbnailUrl , O => O.MapFrom(new CourseThumbnailResolver()))
                                               .ForMember(C => C.CategoryName , O => O.MapFrom(S => S.Category.Name)).ReverseMap();

            CreateMap<CreateCourseRequest, Course>().ReverseMap();
            CreateMap<UpdateCourseRequest, Course>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Course, UpdateCourseRequest>();
        }
    }
}