using System;
using AutoMapper;
using Domain.Entities.Courses;
using Domain.Entities.Orders;
using Shared.DTOS.Orders;

namespace Services.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Course, OrderItemResponse>()
                .ForMember(d => d.CourseId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CourseTitle, opt => opt.MapFrom(s => s.Title))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.GetEffectivePrice(DateTime.UtcNow)))
                .ForMember(d => d.OriginalPrice, opt => opt.MapFrom(s => s.Price));

            CreateMap<OrderItems, OrderItemResponse>();
        }
    }
}
