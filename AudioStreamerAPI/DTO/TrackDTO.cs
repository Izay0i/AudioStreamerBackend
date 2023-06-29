using AudioStreamerAPI.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public class TrackDTO
    {
        [Key]
        public int TrackId { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string TrackName { get; set; } = null!;
        [Required]
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string ArtistName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        [DefaultValue(new string[0])]
        public string[]? Tags { get; set; }
        [DefaultValue(0)]
        public int ViewCountsPerDay { get; set; }
        [DefaultValue(false)]
        public bool HasCaptions { get; set; }
        [DefaultValue(0)]
        public int CaptionsLength { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
