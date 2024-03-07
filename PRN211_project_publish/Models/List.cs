using System;
using System.Collections.Generic;

namespace PRN211_project_publish.Models;

public partial class List
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
