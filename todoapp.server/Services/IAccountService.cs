using todoapp.server.Dtos.AccountDto;

namespace todoapp.server.Services
{
    public interface IAccountService
    {
        AccountResponseDto? GetAccountByUseName(string username);
        
    }
}
