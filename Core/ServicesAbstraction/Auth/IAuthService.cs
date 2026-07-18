using Domain.Entities.Identity;
using Shared.DTOS.Auth;

namespace ServicesAbstraction.Auth
{
    public interface IAuthService
    {
        Task<UserAuthResponse> LoginAsync(LoginRequest request);
        Task<UserAuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct);
        Task<string> GenerateTokenAsync(AppUser user);
    }
}