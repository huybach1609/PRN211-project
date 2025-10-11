using SQLitePCL;
using todoapp.server.Controllers;
using todoapp.server.Models;
using todoapp.server.Services.iml;

namespace todoapp.server.Services
{
    public interface ITaskService
    {
        TaskResponseDTO GetTasksByList(int listId, int userId);
        TaskDto GetTaskById(int taskId);
        todoapp.server.Models.Task UdpateTag(int taskId, todoapp.server.Models.Task taskRequest);
        todoapp.server.Models.Task DeleteTag(int taskId);

        TaskResponseDTO GetTimerTasks(string timeFilter, int userId);

        bool updateStatust(int taskId, bool status);
        TaskAddResponseDTO UpdateTask(TaskAddRequest request);
        TaskAddResponseDTO CreateTask(TaskAddRequest request);
        TaskAddResponseDTO DeleteTask(int userId, int taskId);
        SubTaskResponse CreateSubTask(SubTaskRequest request);
        SubTaskResponse UpdateSubTask(int subtaskId, SubTaskRequest request);
        SubTaskResponse DeleteSubTask(int subtaskId);
    }
}
