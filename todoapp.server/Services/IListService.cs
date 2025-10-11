using todoapp.server.Dtos.ListDto;
using todoapp.server.Models;

namespace todoapp.server.Services
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
