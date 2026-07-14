using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Shared.DTOS.Auth;

namespace Services.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AppUser,RegisterRequest>().ReverseMap();
        }
    }
}