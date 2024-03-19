﻿using System;
using System.Collections.Generic;

namespace hello.Models
{
    public partial class TagsTask
    {
        public int TagsId { get; set; }
        public int TaskId { get; set; }
        public int? Status { get; set; }

        public virtual Tag Tags { get; set; } = null!;
        public virtual Task Task { get; set; } = null!;
    }
}
