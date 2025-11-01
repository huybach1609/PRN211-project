
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NuGet.Packaging;
using SQLitePCL;
using todoapp.server.Controllers;
using todoapp.server.Dtos;
using todoapp.server.Models;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Services.Implementations
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly? DueDate { get; set; }
        public bool? Status { get; set; }
        public int? ListId { get; set; }
        public string ListName { get; set; } // Just the list name
        public DateTime? CreateDate { get; set; }
        public List<SubTaskDto> SubTasks { get; set; } = new List<SubTaskDto>();
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
    }

    public class TaskResponseDTO()
    {
        public List<TaskDto> Tasks { get; set; }
        public string? Message { get; set; }
        public bool Status { get; set; }


    }

    public class SubTaskDto
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
    }

    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TaskService : ITaskService
    {
        private Prn231ProjectContext _context;
        private ObjectMapper _mapper;

        public TaskService(Prn231ProjectContext context, ObjectMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TaskAddResponseDTO CreateTask(TaskAddRequest request)
        {
            Models.Task task = new Models.Task();
            task.Name = request.Name;
            task.Description = request.Description;
            task.ListId = request.ListId;
            task.DueDate = request.DueDate;

            // Add the task to the context
            var taskEntity = _context.Tasks.Add(task).Entity;

            // Save changes
            _context.SaveChanges();


            if (request.SelectedTags != null)
            {
                var newTagTasks = request.SelectedTags.Select(tagId => new TagsTask
                {
                    TagsId = tagId,
                    TaskId = task.Id
                }).ToList();
                _context.TagsTasks.AddRange(newTagTasks);
            }

            _context.SaveChanges();

            var fullTask = _context.Tasks
           .Include(t => t.List)
           .Include(t => t.TagsTasks)
               .ThenInclude(tt => tt.Tags)
           .Include(t => t.SubTasks)
           .FirstOrDefault(t => t.Id == task.Id);



            // Map to DTO
            return new TaskAddResponseDTO()
            {
                Message = $"Created task {task.Name} successfully!",
                Status = true,
                Task = _mapper.ToTaskDto(fullTask),
                // Task = new TaskDto()
                // {
                //     Id = fullTask.Id,
                //     Name = fullTask.Name,
                //     Description = fullTask.Description,
                //     DueDate = fullTask.DueDate,
                //     Status = fullTask.Status,
                //     ListId = fullTask.ListId,
                //     ListName = fullTask.List?.Name,
                //     CreateDate = fullTask.CreateDate,
                //     SubTasks = fullTask.SubTasks.Select(_mapper.ToSubTaskDto).ToList(),
                //     Tags = fullTask.TagsTasks.Select(tt => _mapper.ToTagDto(tt.Tags)).ToList()
                // }
            };
        }




        public TaskAddResponseDTO DeleteTask(int userId, int taskId)
        {
            try
            {
                var task = _context.Tasks
                    .Include(t => t.List)
                    .Include(t => t.SubTasks)
                    .Include(t => t.TagsTasks)
                    .ThenInclude(tt => tt.Tags)
                    .Include(t => t.SubTasks)  // Consider removing related subtasks
                    .FirstOrDefault(t => t.Id == taskId);

                if (task == null)
                {
                    return new TaskAddResponseDTO
                    {
                        Message = "Task not found",
                        Status = false
                    };
                }

                _context.TagsTasks.RemoveRange(task.TagsTasks);
                _context.SubTasks.RemoveRange(task.SubTasks);

                _context.Tasks.Remove(task);
                _context.SaveChanges();

                return new TaskAddResponseDTO
                {
                    Message = $"Removed task {task.Name}",
                    Status = true,
                    Task = new TaskDto
                    {
                        Id = task.Id,
                        Name = task.Name
                    }
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                return new TaskAddResponseDTO
                {
                    Message = $"Error deleting task: {ex.Message}",
                    Status = false
                };
            }
        }
        public TaskDto GetTaskById(int taskId)
        {
            var t = _context.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.List)
                .Include(t => t.TagsTasks)
                .ThenInclude(tt => tt.Tags)
                .FirstOrDefault(t => t.Id == taskId);
            return _mapper.ToTaskDto(t);
            // return new TaskDto()
            // {
            //     Id = t.Id,
            //     Name = t.Name,
            //     Description = t.Description,
            //     DueDate = t.DueDate,
            //     Status = t.Status,
            //     ListId = t.ListId,
            //     ListName = t.List != null ? t.List.Name : null,
            //     CreateDate = t.CreateDate,
            //     SubTasks = t.SubTasks.Select(_mapper.ToSubTaskDto).ToList(),
            //     Tags = t.TagsTasks.Select(tt => _mapper.ToTagDto(tt.Tags)).ToList()
            // };
        }

        public TaskResponseDTO GetTasksByList(int listId, int userId)
        {
            var tasklist = _context.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.List)
                .Include(t => t.TagsTasks)
                .ThenInclude(tt => tt.Tags)
                .Where(t => t.List.AccountId == userId && t.ListId == listId)
                .AsQueryable();

            // Convert to DTOs to avoid circular references
            return new TaskResponseDTO()
            {
                Status = true,
                Message = "Get list task successfully",
                Tasks = tasklist
                .OrderBy(t => t.DueDate)
                .Select(_mapper.ToTaskDto)
                .ToList()
            };
        }

        public TaskResponseDTO GetTimerTasks(string timeFilter, int userId)
        {

            var tasklist = _context.Tasks
                .Include(t => t.List)
                .Include(t => t.SubTasks)
                .Include(t => t.TagsTasks)
                .ThenInclude(tt => tt.Tags)
                .Where(t => t.List.AccountId == userId)
                .AsQueryable();
            // Get today's date as DateOnly for comparison
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            switch (timeFilter.ToLower())
            {
                case "future":
                case "upcoming":
                    tasklist = tasklist.Where(t => t.DueDate.HasValue && t.DueDate > today);
                    break;
                case "today":
                    tasklist = tasklist.Where(t => t.DueDate.HasValue && t.DueDate == today);
                    break;
                case "past":
                case "lated":
                    tasklist = tasklist.Where(t => t.DueDate.HasValue && t.DueDate < today);
                    break;
                default:
                    return new TaskResponseDTO()
                    {
                        Status = false,
                        Message = "Invalid time filter. Use 'future', 'today', or 'past'"
                    };
            }

            // Convert to DTOs to avoid circular references
            return new TaskResponseDTO()
            {
                Status = true,
                Message = "Get list task successfully",
                Tasks = tasklist
                .OrderBy(t => t.DueDate)
                .Select(_mapper.ToTaskDto)
                .ToList()
            };
        }

        public TaskAddResponseDTO UpdateTask(TaskAddRequest request)
        {
            var task = _context.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.List)
                .Include(t => t.TagsTasks)
                .ThenInclude(tt => tt.Tags)
                .FirstOrDefault(t => t.Id == request.TaskId);
            if (task == null)
            {
                return new TaskAddResponseDTO()
                {
                    Message = "Not found",
                    Status = false,
                    Task = null
                };
            }
            task.Name = request.Name;
            task.Description = request.Description;
            task.ListId = request.ListId;
            task.DueDate = request.DueDate;

            // Remove existing tags
            var existingTagTasks = _context.TagsTasks.Where(tt => tt.TaskId == request.TaskId).ToList();
            _context.TagsTasks.RemoveRange(existingTagTasks);

            // Add new tags
            if (request.SelectedTags != null)
            {
                var newTagTasks = request.SelectedTags.Select(tagId => new TagsTask
                {
                    TagsId = tagId,
                    TaskId = request.TaskId
                }).ToList();

                _context.TagsTasks.AddRange(newTagTasks);
            }
           
            _context.SaveChanges();
            return new TaskAddResponseDTO()
            {
                Message = $"update task {task.Name} successfully!",
                Status = true,
                Task = _mapper.ToTaskDto(task)
            };
        }


        bool ITaskService.updateStatust(int taskId, bool status)
        {

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return false;
            }
            task.Status = status;
            _context.SaveChanges();

            return true;
        }

        public Models.Task UdpateTag(int taskId, Models.Task taskRequest)
        {
            throw new NotImplementedException();
        }

        public Models.Task DeleteTag(int taskId)
        {
            throw new NotImplementedException();
        }

        public SubTaskResponse CreateSubTask(SubTaskRequest request)
        {
            Console.WriteLine("request: " + "id: " + request.TaskId + " name:" + request.Name);
            var subtask = _context.SubTasks.Add(new SubTask()
            {
                Id = 0,
                TaskId = request.TaskId,
                Name = request.Name,
                Status = request.Status
            }).Entity;
            Console.WriteLine("subtask: " + "id: " + subtask.TaskId + " name:" + subtask.Name);
            _context.SaveChanges();

            return new SubTaskResponse()
            {
                Status = true,
                Message = "Add substask success",
                SubTask = _mapper.ToSubTaskDto(subtask),
            };
        }

        public SubTaskResponse UpdateSubTask(int subtaskId, SubTaskRequest request)
        {
            try
            {
                // Validate input parameters
                if (subtaskId <= 0)
                {
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = "Invalid subtask ID."
                    };
                }

                if (request == null)
                {
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = "Request cannot be null."
                    };
                }

                // Validate request properties
                if (request.TaskId <= 0)
                {
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = "Invalid task ID."
                    };
                }

                // Check if the subtask exists
                var subtask = _context.SubTasks.FirstOrDefault(t => t.Id == subtaskId);
                if (subtask == null)
                {
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = $"Subtask with ID {subtaskId} not found."
                    };
                }


                // Update the subtask properties
                subtask.TaskId = request.TaskId;
                subtask.Name = request.Name;
                subtask.Status = request.Status;

                // Save changes and handle potential DB errors
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log the exception details here
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = "Database error occurred while updating the subtask."
                    };
                }

                // Return success response
                return new SubTaskResponse()
                {
                    Status = true,
                    Message = "Update subtask success", // Fixed message from "Add" to "Update"
                    SubTask = _mapper.ToSubTaskDto(subtask)
                };
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return new SubTaskResponse()
                {
                    Status = false,
                    Message = "An unexpected error occurred while updating the subtask."
                };
            }
        }
        public SubTaskResponse DeleteSubTask(int subtaskId)
        {
            try
            {
                // Validate input parameter
                if (subtaskId <= 0)
                {
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = "Invalid subtask ID."
                    };
                }

                // Check if the subtask exists
                var subtask = _context.SubTasks.FirstOrDefault(t => t.Id == subtaskId);
                if (subtask == null)
                {
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = $"Subtask with ID {subtaskId} not found."
                    };
                }

                // Store subtask data before removal for the response
                var deletedSubTaskDto = _mapper.ToSubTaskDto(subtask);


                // Remove the subtask
                _context.SubTasks.Remove(subtask);

                // Save changes and handle potential DB errors
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log the exception details here
                    return new SubTaskResponse()
                    {
                        Status = false,
                        Message = "Database error occurred while deleting the subtask."
                    };
                }

                // Return success response
                return new SubTaskResponse()
                {
                    Status = true,
                    Message = "Delete subtask success",
                    SubTask = deletedSubTaskDto
                };
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return new SubTaskResponse()
                {
                    Status = false,
                    Message = "An unexpected error occurred while deleting the subtask."
                };
            }
        }
    }
}
