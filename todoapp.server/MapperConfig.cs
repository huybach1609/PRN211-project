using AutoMapper;
using todoapp.server.Dtos.UserDtos;
using todoapp.server.Models;

namespace todoapp.server
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserResponseDto>();
        }

    }
}
