using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace todoapp.server.Models;

public partial class Task
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool? Status { get; set; }

    public int? ListId { get; set; }

    public DateTime? CreateDate { get; set; }

    [JsonIgnore]
    public virtual List? List { get; set; }

    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    public virtual ICollection<TagsTask> TagsTasks { get; set; } = new List<TagsTask>();
}
