using SQLitePCL;
using todoapp.server.Controllers;
using todoapp.server.Models;
using todoapp.server.Services.Implementations;

namespace todoapp.server.Services.Interfaces
{
    public interface ITaskService
    {
        TaskResponseDTO GetTasksByList(int listId, int userId);
        TaskDto GetTaskById(int taskId);
        Models.Task UdpateTag(int taskId, Models.Task taskRequest);
        Models.Task DeleteTag(int taskId);

        TaskResponseDTO GetTimerTasks(string timeFilter, int userId);

        bool updateStatust(int taskId, bool status);
        TaskAddResponseDTO UpdateTask(TaskAddRequest request);
        TaskAddResponseDTO CreateTask(TaskAddRequest request);
        TaskAddResponseDTO DeleteTask(int userId, int taskId);
        SubTaskResponse CreateSubTask(SubTaskRequest request);
        SubTaskResponse UpdateSubTask(int subtaskId, SubTaskRequest request);
        SubTaskResponse DeleteSubTask(int subtaskId);

        /// <summary>
        /// Get task counts all or by listId
        /// if listid != 0 search by listid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="listId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<TaskCountsDto> GetTaskCountsByTimestampAsync(int userId, int? listId, CancellationToken ct);

    }
}
