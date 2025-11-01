using AutoMapper;
using todoapp.server.Dtos.AccountDtos;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly Prn231ProjectContext _context;
        private readonly IMapper _mapper;
        public AccountService(Prn231ProjectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public AccountResponseDto? GetAccountByUseName(string username)
        {
            var acc = _context.Accounts.FirstOrDefault(acc=> acc.UserName.ToLower().Equals(username.ToLower()));

            return acc == null ? null : _mapper.Map<Account, AccountResponseDto>(acc);
        }
    }
}
