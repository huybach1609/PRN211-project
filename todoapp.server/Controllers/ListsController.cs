using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using todoapp.server.Dtos.ListDtos;
using todoapp.server.Models;
using todoapp.server.Services.Implementations;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly IListService _listService;
        private readonly Prn231ProjectContext _context;
        public ListsController(IListService listService, Prn231ProjectContext context)
        {
            _listService = listService;
            _context = context;

        }

        //============================= OLD ===========================
        // timestamp null // listId = 0
        [Authorize]
        [HttpGet("/api/count-info/{timestamp}/{listId}")]
        public IActionResult GetNumberOfTaskInfo(string timestamp, int listId)
        {
            // Extract userId from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user credentials");
            }

            return Ok((int)_listService.GetNumberOfTaskInfo(timestamp, listId, userId));
        }

        [HttpGet("/api/odata/user/{userId}")]
        [EnableQuery]
        public IActionResult GetListByUserId(int userId)
        {
            return Ok(_listService.GetByUserId(userId));
        }

        [HttpGet("/api/odata/{listId}")]
        public IActionResult GetListById(int listId)
        {
            return Ok((ListResponseDto)_listService.GetListById(listId));
        }
        [EnableQuery]
        [HttpGet("/api/odata")]
        public IActionResult GetAll()
        {
            return Ok(_context.Lists);
        }

        [HttpPost("/api/odata")]
        public IActionResult CreateList([FromBody] ListRequestDto request)
        {
            return Ok((ListResponseDto)_listService.CreateList(request));
        }

        [HttpPut("/api/odata")]
        public IActionResult UdpateList([FromBody] ListRequestDto request)
        {
            return Ok((ListResponseDto)_listService.UpdateList(request));
        }
        //============================= New ===========================
        public record PageResult<T>(IReadOnlyList<T> Items, string? NextCursor);
        public record ListPatchDto(string? Name, string? Description);

        // ===== Helpers =====
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var val = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(val) && int.TryParse(val, out userId);
        }
        private static bool TryDecodeCursor(string? cursor, out int lastId)
        {
            lastId = 0;
            if (string.IsNullOrWhiteSpace(cursor)) return false;
            try
            {
                var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
                return int.TryParse(decoded, out lastId);
            }
            catch { return false; }
        }
        private static string EncodeCursor(int id) =>
           Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(id.ToString()));

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<ListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(
            [FromQuery] int limit = 50,
            [FromQuery] string? cursor = null,
            CancellationToken ct = default)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

            //prevents giant queries.
            limit = Math.Clamp(limit, 1, 200);

            IQueryable<Models.List> q = _context.Lists
              .AsNoTracking()
              .Where(l => l.UserId == userId)
              .OrderBy(l => l.Id);

            //If the client sends cursor, decode it to lastId and continue after that id
            if (TryDecodeCursor(cursor, out var lastId))
                q = q.Where(l => l.Id > lastId);


            var items = await q.Take(limit + 1).ToListAsync(ct);
            var hasMore = items.Count > limit;
            if (hasMore) items.RemoveAt(items.Count - 1);

            var dtoItems = items.Select(t => (ListDto)_listService.MapTaskToResponse(t)).ToList();
            var nextCursor = hasMore ? EncodeCursor(items.Last().Id) : null;
            return Ok(new PageResult<ListDto>(dtoItems, nextCursor));
        }



    }
}


//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace todoapp.server.Controllers
//{
//    [ApiController]
//    [Route("v1/lists")]
//    public class ListsController : ControllerBase
//    {
//        private readonly IListService _listService;
//        private readonly Prn231ProjectContext _context;

//        public ListsController(IListService listService, Prn231ProjectContext context)
//        {
//            _listService = listService;
//            _context = context;
//        }

//        // ===== DTOs for REST shape =====
//        public record PageResult<T>(IReadOnlyList<T> Items, string? NextCursor);
//        public record ListPatchDto(string? Name, string? Description); // add optional fields as needed

//        // ===== Helpers =====
//        private bool TryGetUserId(out int userId)
//        {
//            userId = 0;
//            var val = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            return !string.IsNullOrEmpty(val) && int.TryParse(val, out userId);
//        }

//        private static bool TryDecodeCursor(string? cursor, out int lastId)
//        {
//            lastId = 0;
//            if (string.IsNullOrWhiteSpace(cursor)) return false;
//            try
//            {
//                var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
//                return int.TryParse(decoded, out lastId);
//            }
//            catch { return false; }
//        }

//        private static string EncodeCursor(int id) =>
//            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(id.ToString()));

//        // ====== GET /v1/lists?limit=&cursor= ======
//        [Authorize]
//        [HttpGet]
//        [ProducesResponseType(typeof(PageResult<ListResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetLists(
//            [FromQuery] int limit = 50,
//            [FromQuery] string? cursor = null,
//            CancellationToken ct = default)
//        {
//            if (!TryGetUserId(out var userId))
//                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

//            limit = Math.Clamp(limit, 1, 200);

//            var q = _context.Lists
//                .AsNoTracking()
//                .Where(l => l.OwnerId == userId)
//                .OrderBy(l => l.Id);

//            if (TryDecodeCursor(cursor, out var lastId))
//                q = q.Where(l => l.Id > lastId);

//            var items = await q.Take(limit + 1).ToListAsync(ct);

//            var hasMore = items.Count > limit;
//            if (hasMore) items.RemoveAt(items.Count - 1);

//            var dtoItems = items.Select(l => (ListResponseDto)_listService.MapToResponse(l)).ToList();
//            var nextCursor = hasMore ? EncodeCursor(dtoItems[^1].Id) : null;

//            return Ok(new PageResult<ListResponseDto>(dtoItems, nextCursor));
//        }

//        // ====== GET /v1/lists/{listId} ======
//        [Authorize]
//        [HttpGet("{listId:int}", Name = "GetListById")]
//        [ProducesResponseType(typeof(ListResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetListById(int listId, CancellationToken ct = default)
//        {
//            if (!TryGetUserId(out var userId))
//                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

//            var entity = await _context.Lists
//                .AsNoTracking()
//                .FirstOrDefaultAsync(l => l.Id == listId && l.OwnerId == userId, ct);

//            if (entity is null)
//                return NotFound(new ProblemDetails { Title = "List not found" });

//            return Ok((ListResponseDto)_listService.MapToResponse(entity));
//        }

//        // ====== POST /v1/lists ======
//        [Authorize]
//        [HttpPost]
//        [ProducesResponseType(typeof(ListResponseDto), StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> CreateList([FromBody] ListRequestDto request, CancellationToken ct = default)
//        {
//            if (!TryGetUserId(out var userId))
//                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

//            // ensure the service sets OwnerId = userId (or do it here before passing down)
//            var created = (ListResponseDto)_listService.CreateListForUser(request, userId);
//            // if your service is async, await it

//            return CreatedAtAction(nameof(GetListById),
//                new { listId = created.Id },
//                created);
//        }

//        // ====== PATCH /v1/lists/{listId} ======
//        [Authorize]
//        [HttpPatch("{listId:int}")]
//        [ProducesResponseType(typeof(ListResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> PatchList(int listId, [FromBody] ListPatchDto patch, CancellationToken ct = default)
//        {
//            if (!TryGetUserId(out var userId))
//                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

//            var entity = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId && l.OwnerId == userId, ct);
//            if (entity is null)
//                return NotFound(new ProblemDetails { Title = "List not found" });

//            // apply partial updates
//            if (patch.Name is not null) entity.Name = patch.Name;
//            if (patch.Description is not null) entity.Description = patch.Description;

//            entity.UpdatedAt = DateTime.UtcNow;

//            await _context.SaveChangesAsync(ct);
//            return Ok((ListResponseDto)_listService.MapToResponse(entity));
//        }

//        // ====== DELETE /v1/lists/{listId} ======
//        [Authorize]
//        [HttpDelete("{listId:int}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> DeleteList(int listId, CancellationToken ct = default)
//        {
//            if (!TryGetUserId(out var userId))
//                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

//            var entity = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId && l.OwnerId == userId, ct);
//            if (entity is null)
//                return NotFound(new ProblemDetails { Title = "List not found" });

//            _context.Lists.Remove(entity);
//            await _context.SaveChangesAsync(ct);
//            return NoContent();
//        }

//        // ====== GET /v1/lists/{listId}/tasks ======
//        [Authorize]
//        [HttpGet("{listId:int}/tasks")]
//        [ProducesResponseType(typeof(PageResult<TaskResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetTasksByList(
//            int listId,
//            [FromQuery] int limit = 50,
//            [FromQuery] string? cursor = null,
//            CancellationToken ct = default)
//        {
//            if (!TryGetUserId(out var userId))
//                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

//            var listExists = await _context.Lists.AnyAsync(l => l.Id == listId && l.OwnerId == userId, ct);
//            if (!listExists) return NotFound(new ProblemDetails { Title = "List not found" });

//            limit = Math.Clamp(limit, 1, 200);

//            var q = _context.Tasks
//                .AsNoTracking()
//                .Where(t => t.ListId == listId)
//                .OrderBy(t => t.Id);

//            if (TryDecodeCursor(cursor, out var lastId))
//                q = q.Where(t => t.Id > lastId);

//            var items = await q.Take(limit + 1).ToListAsync(ct);
//            var hasMore = items.Count > limit;
//            if (hasMore) items.RemoveAt(items.Count - 1);

//            var dtoItems = items.Select(t => (TaskResponseDto)_listService.MapTaskToResponse(t)).ToList();
//            var nextCursor = hasMore ? EncodeCursor(dtoItems[^1].Id) : null;

//            return Ok(new PageResult<TaskResponseDto>(dtoItems, nextCursor));
//        }

//        // ====== POST /v1/lists/{listId}/tasks ======
//        [Authorize]
//        [HttpPost("{listId:int}/tasks")]
//        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> CreateTaskInList(
//            int listId,
//            [FromBody] TaskRequestDto request,
//            CancellationToken ct = default)
//        {
//            if (!TryGetUserId(out var userId))
//                return Unauthorized(new ProblemDetails { Title = "Invalid user credentials" });

//            var list = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId && l.OwnerId == userId, ct);
//            if (list is null) return NotFound(new ProblemDetails { Title = "List not found" });

//            // enforce listId from route
//            request = request with { ListId = listId };

//            var created = (TaskResponseDto)_listService.CreateTask(request, userId);
//            // if async, await service

//            // route to your existing tasks controller if you have one; here we point back to this list collection
//            return Created($"/v1/tasks/{created.Id}", created);
//        }

//        // ===== Legacy endpoints kept for reference (remove once migrated) =====
//        // [Authorize]
//        // [HttpGet("/api/count-info/{timestamp}/{listId}")]
//        // public IActionResult GetNumberOfTaskInfo(string timestamp, int listId) { ... }
//    }
//}

