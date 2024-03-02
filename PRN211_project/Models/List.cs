using System;
using System.Collections.Generic;

namespace PRN211_test.Models
{
    public partial class List
    {
        public List()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? AccountId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
