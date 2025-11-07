using todoapp.server.Models;

namespace todoapp.server.Services.Interfaces
{
    public interface IStickyNoteService
    {
        Task<List<StickyNote>> GetStickyNotesByUserId(int userId, CancellationToken ct);
        Task<StickyNote?> GetStickyNoteById(int stId, CancellationToken ct);
        Task<StickyNote?> UpdateStickyNote(int userId,StickyNote note, CancellationToken ct);
        Task<StickyNote?> DeleteStickyNote(int userId, int stId, CancellationToken ct);
        Task<StickyNote?> CreateStickyNote(StickyNote note, CancellationToken ct);
    }
}
