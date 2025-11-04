using System;
using System.Collections.Generic;

namespace todoapp.server.Models;

public partial class StickyNote
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Details { get; set; }

    public virtual User User { get; set; } = null!;
}
