namespace todoapp.server.Dtos.ListDtos
{
    public class ListResponseDto
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public todoapp.server.Models.List? Result { get; set; }
    }
}
