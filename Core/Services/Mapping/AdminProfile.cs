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
            CreateMap<InstructorRequest,InstructorRequestResponse>().ReverseMap();
        }
    }
}