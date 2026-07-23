using AutoMapper;
using Domain.Entities.Cart;
using Domain.Entities.Courses;
using Shared.DTOS.Cart;

namespace Services.Mapping
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Course, CartItemResponse>()
                .ForMember(d => d.CourseId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ThumbnailUrl, o => o.MapFrom(s => 
                    !string.IsNullOrEmpty(s.ThumbnailUrl) 
                        ? $"{Environment.GetEnvironmentVariable("API_BASE_URL")}{s.ThumbnailUrl}" 
                        : string.Empty))
                .ForMember(d => d.Level, o => o.MapFrom(s => s.Level.ToString()))
                .ForMember(d => d.InstructorName, o => o.MapFrom(s => s.Instructor.DisplayName))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
        }
    }
}
