using System;
using System.Collections.Generic;

namespace todoapp.server.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TagsTask> TagsTasks { get; set; } = new List<TagsTask>();
}
