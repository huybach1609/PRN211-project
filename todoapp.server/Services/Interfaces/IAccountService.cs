using todoapp.server.Dtos.AccountDtos;

namespace todoapp.server.Services.Interfaces
{
    public interface IAccountService
    {
        AccountResponseDto? GetAccountByUseName(string username);
        
    }
}
