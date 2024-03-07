using System;
using System.Collections.Generic;

namespace PRN211_project_publish.Models;

public partial class Task
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? Status { get; set; }

    public int? ListId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual List? List { get; set; }

    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
