using todoapp.server.Dtos.UserDtos;

namespace todoapp.server.Services.Interfaces
{
    public interface IUserService
    {
        UserResponseDto? GetUserByUseName(string username);
        UserResponseDto? GetUserByUserId(int userId, CancellationToken ct);
        
    }
}
