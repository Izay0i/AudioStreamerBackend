using System;
using System.Collections.Generic;

namespace AudioStreamerAPI.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Memberstats = new HashSet<Memberstat>();
            Tracks = new HashSet<Track>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; } = null!;
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Memberstat> Memberstats { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
