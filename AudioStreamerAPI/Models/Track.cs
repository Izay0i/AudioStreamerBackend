﻿using System;
using System.Collections.Generic;

namespace AudioStreamerAPI.Models
{
    public partial class Track
    {
        public Track()
        {
            Closedcaptions = new HashSet<Closedcaption>();
            Memberstats = new HashSet<Memberstat>();
        }

        public int TrackId { get; set; }
        public int MemberId { get; set; }
        public string TrackName { get; set; } = null!;
        public string ArtistName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public string[]? Tags { get; set; }
        public int ViewCountsPerDay { get; set; }
        public DateTime DateCreated { get; set; }
        public int CaptionsLength { get; set; }
        public bool HasCaptions { get; set; }
        public int GenreId { get; set; }
        public int ArtistinfoId { get; set; }

        public virtual Artistinfo Artistinfo { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<Closedcaption> Closedcaptions { get; set; }
        public virtual ICollection<Memberstat> Memberstats { get; set; }
    }
}
