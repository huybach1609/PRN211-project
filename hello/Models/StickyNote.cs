using System;
using System.Collections.Generic;

namespace hello.Models
{
    public partial class StickyNote
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
