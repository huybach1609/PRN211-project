using todoapp.server.Models;

namespace todoapp.server.Services.Interfaces
{
    public interface IStickyNoteService
    {
        List<StickyNote> GetStickyNotesByUserId(int userId);
        StickyNote GetStickyNoteById(int stId);
        StickyNote UpdateStickyNote(int stId, StickyNote note);
        StickyNote DeleteStickyNote(int userId, int stId);
        StickyNote CreateStickyNote(StickyNote note);
        
    }
}
