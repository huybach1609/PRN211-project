using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Controllers
{

    public class StickyNoteRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }

    }
    
    [Route("api/[controller]")]
    [ApiController]
    public class StickyNotesController : ControllerBase
    {
        private readonly IStickyNoteService _stickyNoteService;
        private readonly IStickyNoteService _service;
        public StickyNotesController(IStickyNoteService service)
        {
            _service = service;
        }
        [HttpGet("{userId}")]
        public IActionResult GetByUseName(int userId)
        {
            var list = _service.GetStickyNotesByUserId(userId);
            return list != null ? Ok(list) : NotFound();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStickyNote(int id, [FromBody] StickyNoteRequest request)
        {
            if (id != request.Id) return BadRequest(new { status = true, message = "The provided ID does not match the request ID." });

            var obj = _service.UpdateStickyNote(id, new StickyNote()
            {
                Id = request.Id.Value,
                Name = request.Name,
                Details = request.Details
            });

            return Ok(new { status = true, message = "save successfully!" });
        }
        [HttpPost("{userId}")]
        public IActionResult CreateStickNote(int userId,[FromBody]StickyNoteRequest request)
        {
            var obj = _service.CreateStickyNote(new StickyNote()
            {
                UserId = userId,
                Name = request.Name,
                Details= request.Details
            });
            
            return Ok(new { status = true, message = "create successfully!", obj = obj });
        }
        [HttpDelete("{userId}/{snId}")]
        public IActionResult DeleteStickNote(int userId, int snId)
        {
            var obj = _service.DeleteStickyNote(userId,snId);

            return Ok(new { status = true, message = "delete note successfully!", obj = obj });
        }
    }
}
