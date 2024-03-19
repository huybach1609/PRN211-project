using System;
using System.Collections.Generic;

namespace hello.Models
{
    public partial class Tag
    {
        public Tag()
        {
            TagsTasks = new HashSet<TagsTask>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TagsTask> TagsTasks { get; set; }
    }
}
