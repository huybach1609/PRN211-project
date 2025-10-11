using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using todoapp.server.Services;
using todoapp.server.Services.iml;

namespace todoapp.server.Controllers
{
    public class SubTaskResponse
    {

        public bool? Status { get; set; }
        public string? Message { get; set; }
        public SubTaskDto? SubTask { get; set; }

    }
    public class SubTaskRequest
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class SubTasksController : ControllerBase
    {
        public readonly ITaskService _service;
        public SubTasksController(ITaskService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult CreateSubTask([FromBody] SubTaskRequest request)
        {
            return Ok((SubTaskResponse)_service.CreateSubTask(request));
        }

        [HttpPut("{subtaskId}")]
        public IActionResult UpdateSubTask(int subtaskId,[FromBody] SubTaskRequest request)
        {
            return Ok((SubTaskResponse)_service.UpdateSubTask(subtaskId,request));
        }

        [HttpDelete("{subtaskId}")]
        public IActionResult DeleteSubTask(int subtaskId)
        {
            return Ok((SubTaskResponse)_service.DeleteSubTask(subtaskId));
        }


    }
}
