using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Courses;
using Shared.DTOS;
using Shared.DTOS.Courses;

namespace Services.Mapping
{
    public class CourseThumbnailResolver : IValueResolver<Course, CourseResponse, string>
    {
        public string Resolve(Course source, CourseResponse destination, string destMember, ResolutionContext context)
        {
            if(!String.IsNullOrEmpty(source.ThumbnailUrl))
            {
                return $"{Environment.GetEnvironmentVariable("API_BASE_URL")}{source.ThumbnailUrl}";
            }


            return String.Empty;
        }
    }
}