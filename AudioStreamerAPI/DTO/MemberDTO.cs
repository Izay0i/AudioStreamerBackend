using AudioStreamerAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public class MemberDTO
    {
        [Key]
        public int MemberId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_DISPLAY_NAME_LENGTH)]
        public string DisplayName { get; set; } = null!;
        public string NameTag { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
