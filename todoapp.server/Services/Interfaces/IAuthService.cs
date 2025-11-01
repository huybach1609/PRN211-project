using todoapp.server.Dtos.UserDtos;

namespace todoapp.server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse> LoginAsync(UserLoginRequest request, CancellationToken ct);
        Task<UserSignUpResponse> SignUpAsync(UserSignUpRequest request, CancellationToken ct);
        Task<bool> ValidateTokenAsync(string token, CancellationToken ct);
        Task<UserLoginResponse> ForgotPasswordAsync(string key, CancellationToken ct);
        Task<bool> ResetPasswordAsync(string password, string repassword, string token, CancellationToken ct);
        string CheckToken(string token);
    }
}
