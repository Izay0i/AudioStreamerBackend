using System;
using System.Collections.Generic;

namespace AudioStreamerAPI.Models
{
    public partial class Member
    {
        public Member()
        {
            Memberstats = new HashSet<Memberstat>();
            Playlists = new HashSet<Playlist>();
            Tracks = new HashSet<Track>();
        }

        public int MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string NameTag { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public int[]? FollowingIds { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Memberstat> Memberstats { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
