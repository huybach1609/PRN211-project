using System.Security.Cryptography.Pkcs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using todoapp.server.Models;
using todoapp.server.Services.Implementations;

namespace todoapp.server.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly TagService _service;
        public TagsController(TagService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetListByUserId(int userId)
        {
            List<Tag> tags = _service.GetTagByUserId(userId);
            return tags != null ? Ok(tags) : NoContent();
        }

    }
}
