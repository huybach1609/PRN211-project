using System;
using System.Collections.Generic;

namespace todoapp.server.Models;

public partial class List
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
