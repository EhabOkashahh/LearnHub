using AutoMapper;
using Domain.Entities.Identity;
using Shared.DTOS.Users;

namespace Services.Mapping
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<AppUser, UserResponse>();
            CreateMap<UpdateUserRequest, AppUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
