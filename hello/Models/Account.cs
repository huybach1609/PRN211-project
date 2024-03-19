using System;
using System.Collections.Generic;

namespace hello.Models
{
    public partial class Account
    {
        public Account()
        {
            Lists = new HashSet<List>();
            StickyNotes = new HashSet<StickyNote>();
        }

        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? Roll { get; set; }

        public virtual ICollection<List> Lists { get; set; }
        public virtual ICollection<StickyNote> StickyNotes { get; set; }
    }
}
