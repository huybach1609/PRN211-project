using todoapp.server.Models;
using todoapp.server.Services.Implementations;

namespace todoapp.server.Dtos;
public class ObjectMapper
{

    public ObjectMapper()
    {

    }
    public SubTaskDto ToSubTaskDto(SubTask subTask)
    {
        return new SubTaskDto
        {
            Id = subTask.Id,
            TaskId = subTask.TaskId,
            Name = subTask.Name ?? "",
            Status = subTask.Status
        };
    }
    public TagDto ToTagDto(Tag ob)
    {
        return new TagDto()
        {
            Id = ob.Id,
            Name = ob.Name
        };
    }
    public TaskDto ToTaskDto(todoapp.server.Models.Task task)
    {
        return new TaskDto()
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            ListId = task.ListId,
            ListName = task.List != null ? task.List.Name : null,
            CreateDate = task.CreateDate,
            SubTasks = task.SubTasks.Select(ToSubTaskDto).ToList(),
            Tags = task.TagsTasks.Select(tt => ToTagDto(tt.Tags)).ToList(),
        };
    }

}