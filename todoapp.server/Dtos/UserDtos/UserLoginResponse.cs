using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using todoapp.server.Models;

namespace todoapp.server.Dtos.UserDtos
{
    public class UserLoginResponse : MessageReturn
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
