﻿using System;
using System.Collections.Generic;

namespace PRN211_project.Models
{
    public partial class Task
    {
        public Task()
        {
            SubTasks = new HashSet<SubTask>();
            TagsTasks = new HashSet<TagsTask>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Status { get; set; }
        public int? ListId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual List? List { get; set; }
        public virtual ICollection<SubTask> SubTasks { get; set; }
        public virtual ICollection<TagsTask> TagsTasks { get; set; }
    }
}
