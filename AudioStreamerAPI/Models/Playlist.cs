using System;
using System.Collections.Generic;

namespace AudioStreamerAPI.Models
{
    public partial class Playlist
    {
        public int PlaylistId { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int[]? TracksIds { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
