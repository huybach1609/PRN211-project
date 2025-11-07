using System.Reflection;
using Microsoft.EntityFrameworkCore;
using todoapp.server.Dtos.ListDtos;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Services.Implementations
{
    /// <summary>
    /// Task count summary for a given list.
    /// </summary>
    public class TaskCountsDto
    {
        /// <summary>Total number of tasks in the query.</summary>
        public int Total { get; set; } = 0;
        /// <summary>Number of tasks due today.</summary>
        public int Today { get; set; } = 0;
        /// <summary>Number of upcoming tasks.</summary>
        public int Upcoming { get; set; } = 0;
        /// <summary>Number of overdue tasks.</summary>
        public int Overdue { get; set; } = 0;
        /// <summary>Number of completed tasks.</summary>
        public int Completed { get; set; } = 0;

    }
    public class ListService : IListService
    {
        private readonly Prn231ProjectContext _context;
        public ListService(Prn231ProjectContext context)
        {
            _context = context;
        }
        public async Task<System.Collections.Generic.List<ListResponseDto>> GetListsByUserIdAsync(int userId, bool includeCount, CancellationToken ct)
        {
            var listquery = _context.Lists
                .AsNoTracking()
                .Where(l => l.UserId == userId)
                .OrderBy(l => l.Id);

            return await listquery.Select(l => new ListResponseDto
            {
                Result = l,
                NumberOfTaskInfo = includeCount ? _context.Tasks.Count(t => t.ListId == l.Id) : null
            }).ToListAsync(ct);
        }

        public Task<List?> GetListByIdAsync(int listId, CancellationToken ct)
        {
            return _context.Lists.AsNoTracking().FirstOrDefaultAsync(l => l.Id == listId, ct);
        }

        public async Task<List> CreateListAsync(int userId, string? name, CancellationToken ct)
        {
            var entity = new List
            {
                UserId = userId,
                Name = name
            };
            _context.Lists.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<List?> UpdateListAsync(int userId, int listId, string? name, CancellationToken ct)
        {
            var entity = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId, ct);
            if (entity == null) return null;

            if (name != null) entity.Name = name;
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<List?> DeleteListAsync(int userId, int listId, CancellationToken ct)
        {
            var entity = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId, ct);
            if (entity == null) return null;
            _context.Lists.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}
