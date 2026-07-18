using Shared.DTOS.Users;

namespace ServicesAbstraction.Users
{
    public interface IUsersService
    {
        Task<UserResponse> GetByIdAsync(string id, CancellationToken ct);
        Task UpdateProfileAsync(string userId, UpdateUserRequest request, CancellationToken ct);
        Task RequestInstructorAsync(string userId, CancellationToken ct);
    }
}
