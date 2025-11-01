using todoapp.server.Dtos.ListDtos;
using todoapp.server.Models;

namespace todoapp.server.Services.Interfaces
{
    public interface IListService
    {
        ListResponseDto CreateList(ListRequestDto request);
        IEnumerable<List> GetByUserId(int userId);
        ListResponseDto GetListById(int listId);
        int GetNumberOfTaskInfo(string timestamp, int listId, int userId);
        ListResponseDto UpdateList(ListRequestDto request);
    }
}
