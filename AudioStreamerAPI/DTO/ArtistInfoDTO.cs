using AudioStreamerAPI.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public class ArtistInfoDTO
    {
        [Key]
        public int ArtistinfoId { get; set; }
        [Required]
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_DISPLAY_NAME_LENGTH)]
        public string ArtistName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string MainSiteAddress { get; set; } = null!;
    }
}
