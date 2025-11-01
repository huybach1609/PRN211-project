using System.Reflection;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using todoapp.server.Dtos.ListDtos;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Services.Implementations
{
    public class ListService : IListService
    {
        private readonly Prn231ProjectContext _context;
        public ListService(Prn231ProjectContext context)
        {
            _context = context;
        }

        public ListResponseDto CreateList(ListRequestDto request)
        {
            var list = _context.Lists.Add(new todoapp.server.Models.List()
            {
                Name = request.Name,
                AccountId = request.AccountId
            }).Entity;
            _context.SaveChanges();
            return new ListResponseDto
            {
                Message = $"Create list {request.Name} successfully",
                Status = true,
                Result = list
            };
        }

        public IEnumerable<List> GetByUserId(int userId)
        {
            IEnumerable<List> list = _context.Lists
                .Include(l => l.Tasks)
                .Where(l => l.AccountId == userId)
                .AsQueryable();
            return list;
        }

        public ListResponseDto GetListById(int listId)
        {
            var listd = _context.Lists.FirstOrDefault(x => x.Id == listId);
            if (listd == null)
            {
                return new ListResponseDto
                {
                    Message = $"No found list ",
                    Status = false,
                    Result = new List()
                };
            }
            return new ListResponseDto
            {
                Message = $"found list ",
                Status = true,
                Result = listd
            };
        }

        // if listid != 0 search by listid
        public int GetNumberOfTaskInfo(string timestamp, int listId, int userId)
        {
            if (listId != 0)
            {
                var listinfo = _context.Lists.Include(l => l.Tasks)
                    .FirstOrDefault(l => l.Id == listId);
                return listinfo.Tasks.Count();
            }
            else if (!string.IsNullOrEmpty(timestamp))
            {

                var tasklist = _context.Tasks
                    .Include(t => t.List)
                    .Where(t => t.List.AccountId == userId)
                    .AsQueryable();
                // Get today's date as DateOnly for comparison
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                switch (timestamp.ToLower())
                {
                    case "future":
                    case "upcoming":
                        tasklist = tasklist.Where(t => t.DueDate.HasValue && t.DueDate > today);
                        break;
                    case "today":
                        tasklist = tasklist.Where(t => t.DueDate.HasValue && t.DueDate == today);
                        break;
                    case "past":
                    case "lated":
                        tasklist = tasklist.Where(t => t.DueDate.HasValue && t.DueDate < today);
                        break;
                    default:
                        return 0;
                }
                return tasklist != null ? tasklist.ToList().Count() : 0;
            }
            return 0;
        }

        public ListResponseDto UpdateList(ListRequestDto request)
        {

            var listdb = _context.Lists.FirstOrDefault(l => l.Id == request.Id);

            if (listdb == null)
            {
                return new ListResponseDto
                {
                    Message = $"No found list {request.Name}",
                    Status = false,
                };
            }

            listdb.Name = request.Name;

            _context.SaveChanges();

            return new ListResponseDto
            {
                Message = $"Update list {request.Name} successfully",
                Status = true,
                Result = listdb
            };
        }
    }
}
