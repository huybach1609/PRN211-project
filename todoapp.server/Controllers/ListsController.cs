using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using todoapp.server.Dtos.ListDtos;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Controllers
{
    [Route("odata/[controller]")]
    //[Route("api/[controller]")]
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



        // timestamp null // listId = 0
        [Authorize]
        [HttpGet("/count-info/{timestamp}/{listId}")]
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


        [HttpGet("user/{userId}")]
        [EnableQuery]
        public IActionResult GetListByUserId(int userId)
        {
            return Ok(_listService.GetByUserId(userId));
        }

        [HttpGet("{listId}")]
        public IActionResult GetListById(int listId)
        {
            return Ok((ListResponseDto)_listService.GetListById(listId));
        }
        [EnableQuery]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Lists);
        }

        [HttpPost]
        public IActionResult CreateList([FromBody] ListRequestDto request)
        {
            return Ok((ListResponseDto)_listService.CreateList(request));
        }

        [HttpPut]
        public IActionResult UdpateList([FromBody] ListRequestDto request)
        {
            return Ok((ListResponseDto)_listService.UpdateList(request));
        }


    }
}
