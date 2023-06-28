using AudioStreamerAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public partial class CredentialsDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(LengthConstants.MIN_PASSWORD_LENGTH)]
        public string Password { get; set; } = string.Empty;
        [MinLength(LengthConstants.MIN_NAME_LENGTH), MaxLength(LengthConstants.MAX_DISPLAY_NAME_LENGTH)]
        public string DisplayName { get; set; } = string.Empty;
    }
}
