using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using todoapp.server.Models;

namespace todoapp.server.Services.Implementations
{
    public class TagService
    {
        private readonly Prn231ProjectContext _context;
        public TagService(Prn231ProjectContext context)
        {
            _context = context;
        }

        public List<Tag> GetTagByUserId(int userId)
        {
            var tags = _context.Tags.Where(tag => tag.TagsTasks
                .Any(tt => tt.Task.List != null && tt.Task.List.AccountId == userId))
                .ToList();
            return tags;
        }
    }
}