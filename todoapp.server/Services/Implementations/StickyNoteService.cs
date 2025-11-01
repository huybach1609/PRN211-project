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

        public List<StickyNote> GetStickyNotesByUserId(int userid)
        {
            var list = _context.StickyNotes.Where(c => c.AccountId == userid).ToList();
            return list;
        }
        public StickyNote? DeleteStickyNote(int userId, int stId)
        {

            var st = _context.StickyNotes.FirstOrDefault(c => c.Id == stId);
            if (st != null)
            {
                if (st.AccountId == userId)
                {
                    _context.StickyNotes.Remove(st);
                    _context.SaveChanges();
                }
            }
            return st;
        }

        public StickyNote GetStickyNoteById(int stId)
        {
            return _context.StickyNotes.FirstOrDefault(c => c.Id == stId);
        }


        public StickyNote UpdateStickyNote(int stId, StickyNote note)
        {
            var st = _context.StickyNotes.FirstOrDefault(c => c.Id == stId);
            if (st != null)
            {
                st.Name = note.Name;
                st.Details = note.Details;
                _context.StickyNotes.Update(st);
                _context.SaveChanges();
            }
            return st;
        }

        public StickyNote CreateStickyNote(StickyNote note)
        {
            StickyNote st = _context.StickyNotes.Add(note).Entity;
            _context.SaveChanges();
            return st;
        }
    }
}
