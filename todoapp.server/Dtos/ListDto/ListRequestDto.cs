using System.Reflection.Metadata.Ecma335;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace todoapp.server.Dtos.ListDto
{
    public class ListRequestDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? Name { get; set; }
    }
}
