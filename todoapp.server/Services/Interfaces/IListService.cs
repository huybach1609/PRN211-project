using todoapp.server.Dtos.ListDtos;
using todoapp.server.Models;

namespace todoapp.server.Services.Interfaces
{
    public interface IListService
    {
        //ListResponseDto CreateList(ListRequestDto request);
        //IEnumerable<List> GetByUserId(int userId);
        //ListResponseDto GetListById(int listId);
        //ListDto MapTaskToResponse(List t);
        //ListResponseDto UpdateList(ListRequestDto request);

        // ===== New async, REST-style members =====
        Task<System.Collections.Generic.List<ListResponseDto>> GetListsByUserIdAsync(int userId,bool includeCounts, CancellationToken ct);
        Task<List?> GetListByIdAsync(int listId, CancellationToken ct);
        Task<List> CreateListAsync(int userId, string? name, CancellationToken ct);
        Task<List?> UpdateListAsync(int userId, int listId, string? name, CancellationToken ct);
        Task<List?> DeleteListAsync(int userId, int listId, CancellationToken ct);
    }
}
