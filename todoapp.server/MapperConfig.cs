using AutoMapper;
using todoapp.server.Dtos.AccountDto;
using todoapp.server.Models;

namespace todoapp.server
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Account, AccountResponseDto>();
            
        }

    }
}
