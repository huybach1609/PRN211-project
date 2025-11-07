using System.Security.Claims;
using System.Security.Cryptography.Pkcs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using todoapp.server.Models;
using todoapp.server.Services.Implementations;

namespace todoapp.server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly TagService _service;
        public TagsController(TagService service)
        {
            _service = service;
        }

        [HttpGet()]
        public IActionResult GetListByUserId()
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier));
                List<Tag> tags = _service.GetTagByUserId(userId);
                return tags != null ? Ok(tags) : NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
