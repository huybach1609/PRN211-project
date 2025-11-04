using todoapp.server.Models;

namespace todoapp.server.Dtos.ListDtos
{
    public class ListDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? UserId { get; set; }
    }
}