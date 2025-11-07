using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using todoapp.server.Models;
using todoapp.server.Services.Implementations;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Controllers
{

    public class StickyNoteRequest
    {
        [Required(ErrorMessage = "Empty sticky note id")]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Invalid sticky note data")]
        public string? Name { get; set; }
        public string? Details { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StickyNotesController : ControllerBase
    {
        private readonly IStickyNoteService _service;
        public StickyNotesController(IStickyNoteService service)
        {
            _service = service;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user identifier");
            }
            return userId;

        }

        // GET: api/stickynotes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStickyNoteById(CancellationToken ct)
        {
            try
            {
                // add try catch for convert
                var userId = GetCurrentUserId();
                var notes = await _service.GetStickyNotesByUserId(userId, ct);
                return Ok(notes);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // GET: api/stickynotes/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StickyNote>> GetById(int id, CancellationToken ct)
        {
            try
            {
                var userId = GetCurrentUserId();
                var note = await _service.GetStickyNoteById(id, ct);

                if (note == null)
                {
                    return NotFound(new { message = "Sticky note not found" });
                }

                if (note.UserId != userId)
                {
                    return Forbid();
                }

                return Ok(note);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
        // POST: api/stickynotes
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] StickyNoteRequest request, CancellationToken ct)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notes = await _service.CreateStickyNote(new StickyNote()
                {
                    UserId = userId,
                    Name = request.Name,
                    Details = request.Details
                }, ct);

                return CreatedAtAction(
                     nameof(GetById),
                     new { id = notes.Id },
                       notes
                     );
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // PUT: api/stickynotes/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateStickyNote(int id, [FromBody] StickyNoteRequest request, CancellationToken ct)
        {
            try
            {
                var userId = GetCurrentUserId();
                var note = await _service.UpdateStickyNote(userId,
                    new StickyNote()
                    {
                        Id = request.Id.Value,
                        Name = request.Name,
                        Details = request.Details
                    }, ct);
                if (note == null) return NotFound(new { message = "Sticky note not found or access denied" });
                return Ok(note);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();

            }
        }

        // DELETE: api/stickynotes/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteStickNote(int id, CancellationToken ct)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var obj = await _service.DeleteStickyNote(userId, id, ct);
                if (obj == null) return NotFound(new { message = "Sticky note not found or access denied" });
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }

        }
    }
}
