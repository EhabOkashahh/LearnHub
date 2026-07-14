using Shared.DTOS.Auth;

namespace ServicesAbstraction
{
    public interface IAuthService
    {
        Task<UserAuthResponse> LoginAsync(LoginRequest request);
        Task<UserAuthResponse> RegisterAsync(RegisterRequest request);
    }
}