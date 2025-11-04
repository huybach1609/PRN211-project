using System;
using System.Collections.Generic;

namespace todoapp.server.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public int? Role { get; set; }

    public virtual ICollection<List> Lists { get; set; } = new List<List>();

    public virtual ICollection<StickyNote> StickyNotes { get; set; } = new List<StickyNote>();
}
