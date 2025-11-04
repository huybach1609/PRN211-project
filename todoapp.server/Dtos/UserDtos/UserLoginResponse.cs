using System.Reflection.Metadata.Ecma335;
using todoapp.server.Models;

namespace todoapp.server.Dtos.UserDtos
{
    public class UserLoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Key { get; set; }
        public User? User { get; set; }
    }
}
