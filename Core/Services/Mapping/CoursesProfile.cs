using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Courses;
using Shared.DTOS;

namespace Services.Mapping
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseResponse>().ForMember(D => D.ThumbnailUrl , O => O.MapFrom(new CourseThumbnailResolver())).ReverseMap();
            CreateMap<CreateCourseRequest, Course>().ReverseMap();
            CreateMap<UpdateCourseRequest, Course>().ReverseMap();
        }
    }
}