using Microsoft.EntityFrameworkCore;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Services.Implementations
{
    public class StickyNoteService : IStickyNoteService
    {
        private readonly Prn231ProjectContext _context;
        public StickyNoteService(Prn231ProjectContext context)
        {
            _context = context;
        }

        public async Task<List<StickyNote>> GetStickyNotesByUserId(int userid, CancellationToken ct)
        {

            var list = await _context.StickyNotes.Where(c => c.UserId == userid).ToListAsync();
            return list;
        }
        public async Task<StickyNote?> DeleteStickyNote(int userId, int stId, CancellationToken ct)
        {

            var st = await _context.StickyNotes.FirstOrDefaultAsync(c => c.Id == stId && c.UserId == userId);
            if (st == null) return null;

            _context.StickyNotes.Remove(st);
            _context.SaveChanges();
            return st;
        }

        public Task<StickyNote?> GetStickyNoteById(int stId, CancellationToken ct)
        {
            return _context.StickyNotes.FirstOrDefaultAsync(c => c.Id == stId);
        }


        public async Task<StickyNote?> UpdateStickyNote(int userId, StickyNote note, CancellationToken ct)
        {
            var st = await _context.StickyNotes.FirstOrDefaultAsync(c => c.Id == note.Id && userId == c.UserId);
            if (st == null) return null;

            st.Name = note.Name;
            st.Details = note.Details;
            _context.StickyNotes.Update(st);
            await _context.SaveChangesAsync();
            return st;
        }

        public async Task<StickyNote?> CreateStickyNote(StickyNote note, CancellationToken ct)
        {
            var st = _context.StickyNotes.Add(note).Entity;
            await _context.SaveChangesAsync();
            return st;
        }
    }
}
