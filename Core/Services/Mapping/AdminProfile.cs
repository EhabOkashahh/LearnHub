using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Shared.DTOS.Admin;

namespace Services.Mapping
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<InstructorRequest, InstructorRequestResponse>()
                .ForMember(d => d.RequestedAt, o => o.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.ReviewedAt, o => o.MapFrom(s => s.UpdatedAt))
                .ReverseMap();
        }
    }
}