using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using todoapp.server.Models;
using todoapp.server.Services.Implementations;
using todoapp.server.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace todoapp.server.Controllers
{

    public class TaskAddRequest()
    {
        //    name: name, // string
        //    description: description, //string
        //    listId: listId, // int
        //    dueDate: dueDate,// dateonly
        //    selectedTags: Array.from(selectedTags).map(Number), // list<int>
        public int TaskId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ListId { get; set; }
        public DateOnly DueDate { get; set; }
        public List<int>? SelectedTags { get; set; }
    }
    public class TaskAddResponseDTO()
    {
        public TaskDto Task { get; set; }
        public string? Message { get; set; }
        public bool Status { get; set; }

    }
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }



        [Authorize]
        [HttpGet("get/{taskId}")]
        public IActionResult GetByTaskId(int taskId)
        {
            return Ok(_taskService.GetTaskById(taskId));
        }

        // Get: /api/tasks/list/{listId} 
        [Authorize]
        [HttpGet("list/{listId}")]
        public IActionResult GetTaskByListId(int listId)
        {

            // Extract userId from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user credentials");
            }

            return Ok(_taskService.GetTasksByList(listId, userId));
        }


        // GET: api/Tasks/{timeFilter}
        [Authorize]
        [HttpGet("{timeFilter}")]
        public IActionResult GetByTime(string timeFilter)
        {
            if (string.IsNullOrEmpty(timeFilter))
            {
                return BadRequest("Time filter cannot be empty");
            }
            // Extract userId from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user credentials");
            }

            var response = _taskService.GetTimerTasks(timeFilter, userId);


            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("updatestatus/{taskId}/{status}")]
        public IActionResult UpdateStatus(int taskId, bool status)
        {
            return Ok(_taskService.updateStatust(taskId, status));

        }

        // POST api/<TasksController>
        [HttpPost]
        public IActionResult Post([FromBody] TaskAddRequest request)
        {
            return Ok((TaskAddResponseDTO)_taskService.CreateTask(request));
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TaskAddRequest request)
        {
            return Ok((TaskAddResponseDTO)_taskService.UpdateTask(request));
        }

        // DELETE api/<TasksController>/5
        [Authorize]
        [HttpDelete("{taskId}")]
        public IActionResult Delete(int taskId)
        {
            // Extract userId from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user credentials");
            }

            return Ok((TaskAddResponseDTO)_taskService.DeleteTask(userId, taskId));
        }
    }
}
