using System;
using System.Collections.Generic;

namespace PRN211_test.Models
{
    [Serializable]
    public partial class Account
    {
        public Account()
        {
            Lists = new HashSet<List>();
        }

        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? Roll { get; set; }

        public virtual ICollection<List> Lists { get; set; }
    }
}
