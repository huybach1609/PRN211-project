using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using todoapp.server.Services.Implementations;
using todoapp.server.Services.Interfaces;


namespace todoapp.server.Controllers
{

    public class TaskAddRequest()
    {
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
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }



        /// <param name="listId">List Id that contains list of tasks.(optional)</param>
        /// <param name="ct">Cancellation token (optional).</param>
        /// <returns>Returns counts of total, today, upcoming, overdue, and completed tasks.</returns>
        [Authorize]
        [HttpGet("task-count")]
        [ProducesResponseType(typeof(TaskCountsDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTaskCount(
            [FromQuery] int listId
            , CancellationToken ct = default)
        {
            try
            {
                var userIdClaim = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return
                    Ok(await _taskService.GetTaskCountsByTimestampAsync(userIdClaim, listId, ct));
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
