using System;
using System.Collections.Generic;

namespace AudioStreamerAPI.Models
{
    public partial class Artistinfo
    {
        public Artistinfo()
        {
            Tracks = new HashSet<Track>();
        }

        public int ArtistinfoId { get; set; }
        public string ArtistName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public int[]? TracksIds { get; set; }
        public DateTime DateCreated { get; set; }
        public string MainSiteAddress { get; set; } = null!;

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
