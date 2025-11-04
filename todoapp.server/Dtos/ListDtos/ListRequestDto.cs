using System.Reflection.Metadata.Ecma335;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace todoapp.server.Dtos.ListDtos
{
    public class ListRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
    }
}
