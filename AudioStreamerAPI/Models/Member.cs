using AudioStreamerAPI.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AudioStreamerAPI.Models
{
    public partial class Member
    {
        public Member()
        {
            Playlists = new HashSet<Playlist>();
        }

        [Key]
        public int MemberId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        [MinLength(LengthConstants.MIN_PASSWORD_LENGTH)]
        public string Password { get; set; } = null!;
        public string Token { get; set; } = null!;
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_DISPLAY_NAME_LENGTH)]
        public string DisplayName { get; set; } = null!;
        public string NameTag { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        [DefaultValue(new int[0])]
        public int[]? FollowingIds { get; set; }
        [JsonIgnore]
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
