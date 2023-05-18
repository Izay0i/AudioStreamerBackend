using System;
using System.Collections.Generic;

namespace AudioStreamerAPI.Models
{
    public partial class Memberstat
    {
        public int MemberId { get; set; }
        public int TrackId { get; set; }
        public int ViewCountsTotal { get; set; }
        public int Rating { get; set; }
        public string[]? Tags { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Track Track { get; set; } = null!;
    }
}
