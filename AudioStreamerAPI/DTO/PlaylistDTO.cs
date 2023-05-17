using AudioStreamerAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AudioStreamerAPI.DTO
{
    public class PlaylistDTO
    {
        [Key]
        public int PlaylistId { get; set; }
        public int MemberId { get; set; }
        [Required]
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; } = null!;
        [MaxLength(LengthConstants.MAX_NAME_LENGTH)]
        public string Description { get; set; } = null!;
        [DefaultValue(new int[0])]
        public int[]? TracksIds { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
