using System;
using System.Collections.Generic;

namespace AudioStreamerAPI.Models
{
    public partial class Closedcaption
    {
        public int CaptionId { get; set; }
        public int TrackId { get; set; }
        public string Captions { get; set; } = null!;
        public DateTime DateCreated { get; set; }

        public virtual Track Track { get; set; } = null!;
    }
}
