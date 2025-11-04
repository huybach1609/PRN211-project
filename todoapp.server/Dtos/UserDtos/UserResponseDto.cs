using todoapp.server.Models;

namespace todoapp.server.Dtos.UserDtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public int? Roll { get; set; }

    }
}
