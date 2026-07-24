using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Domain.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Courses.Enums;
using Services.Specifications.UserSpecifications;
using ServicesAbstraction.Auth;
using ServicesAbstraction.Users;
using Shared.DTOS.Users;
using Domain.Exceptions.BadRequestExceptions;

namespace Services
{
    public class UserService(
        UserManager<AppUser> _userManager,
        IMapper _mapper,
        IUnitOfWork _uof,
        IAuthService _auth
    ) : IUsersService
    {
        public async Task<UserResponse> GetByIdAsync(string id, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) throw new UserNotFoundException(id);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task UpdateProfileAsync(string userId, UpdateUserRequest request, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new UserNotFoundException(userId);
            _mapper.Map(request, user);
            await _userManager.UpdateAsync(user);
        }

        public async Task RequestInstructorAsync(string userId, CancellationToken ct)
        {
            var spec = new InstructorSpecifications(userId);
            var UserRequest = await _uof.GetRepository<Guid,InstructorRequest>().GetAsync(spec,ct);

            if(UserRequest is not null)
            {
                if(UserRequest.Status is RequestStatus.Pending) throw new BadRequestException("You already have a pending Request");

                else if(UserRequest.Status is RequestStatus.Rejected && (DateTime.Now - UserRequest.UpdatedAt) < TimeSpan.FromDays(30)) 
                    throw new BadRequestException("You must wait 30 days before re-applying for instructor role");
            }

            var request = new InstructorRequest()
            {
                UserId = userId,
            };

            await _uof.GetRepository<Guid,InstructorRequest>().AddAsync(request);
            await _uof.SaveChangesAsync(ct);
        }

        public async Task<TokenResponse> RefreshTokenAsync(string userId, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new UserNotFoundException(userId);

            var token = await _auth.GenerateTokenAsync(user);
            return new TokenResponse { Token = token };
        }
    }
}
