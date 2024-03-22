using System;
using System.Collections.Generic;

namespace PRN211_project.Models
{
    public partial class SubTask
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }

        public virtual Task? Task { get; set; }
    }
}
