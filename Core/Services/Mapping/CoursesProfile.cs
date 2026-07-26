using AutoMapper;
using Domain.Entities.Courses;
using Shared.DTOS;
using Shared.DTOS.Courses;

namespace Services.Mapping
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseResponse>().ForMember(D => D.ThumbnailUrl , O => O.MapFrom(new CourseThumbnailResolver()))
                                               .ForMember(C => C.CategoryName , O => O.MapFrom(S => S.Category.Name))
                                               .ForMember(C => C.Status, O => O.MapFrom(S => S.Status.ToString()))
                                               .ForMember(C => C.InstructorName, O => O.MapFrom(S => S.Instructor.DisplayName))
                                               .ForMember(C => C.Sections, o => { o.Condition(d => d.CourseSections != null); o.MapFrom(d => d.CourseSections); })
                                               .ReverseMap();

            CreateMap<CourseSection, CourseSectionDTO>().ReverseMap();
            CreateMap<Lesson, LessonDTO>().ReverseMap();

            CreateMap<CreateCourseSectionRequest, CourseSection>();
            CreateMap<UpdateCourseSectionRequest, CourseSection>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateLessonRequest, Lesson>();
            CreateMap<UpdateLessonRequest, Lesson>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateCourseRequest, Course>().ReverseMap();
            CreateMap<UpdateCourseRequest, Course>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Course, UpdateCourseRequest>();
        }
    }
}