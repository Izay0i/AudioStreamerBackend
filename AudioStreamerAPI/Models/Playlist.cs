using AudioStreamerAPI.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AudioStreamerAPI.Models
{
    public partial class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }
        public int MemberId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; } = null!;
        [MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string Description { get; set; } = null!;
        [DefaultValue(new int[0])]
        public int[]? TracksIds { get; set; }
        [JsonIgnore]
        public virtual Member Member { get; set; } = null!;
    }
}
