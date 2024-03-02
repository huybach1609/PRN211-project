using System;
using System.Collections.Generic;

namespace PRN211_test.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
