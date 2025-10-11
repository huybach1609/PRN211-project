using todoapp.server.Models;

namespace todoapp.server.Dtos.AccountDto
{
    public class AccountResponseDto
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public int? Roll { get; set; }

    }
}
