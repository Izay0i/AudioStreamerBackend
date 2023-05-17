using AudioStreamerAPI.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.Models
{
    public partial class Track
    {
        [Key]
        public int TrackId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string TrackName { get; set; } = null!;
        [MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public int[] AuthorsIds { get; set; } = null!;
        [DefaultValue(new string[0])]
        public string[]? Tags { get; set; }
        public DateTime DateUploaded { get; set; }
        public int ViewCountPerDay { get; set; }
        public int ViewCountTotal { get; set; }
    }
}
