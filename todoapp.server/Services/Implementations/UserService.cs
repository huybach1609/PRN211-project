using AutoMapper;
using todoapp.server.Dtos.UserDtos;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly Prn231ProjectContext _context;
        private readonly IMapper _mapper;
        public UserService(Prn231ProjectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public UserResponseDto? GetUserByUseName(string username)
        {
            var acc = _context.Users.FirstOrDefault(acc=> acc.UserName.ToLower().Equals(username.ToLower()));
            return acc == null ? null : _mapper.Map<User, UserResponseDto>(acc);
        }

        public UserResponseDto? GetUserByUserId(int userId, CancellationToken t)
        {
            var acc = _context.Users.FirstOrDefault(u => u.Id == userId);
            return acc == null ? null : _mapper.Map<User, UserResponseDto>(acc);
        }
    }
}
