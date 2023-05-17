using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public partial class CredentialsDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(14)]
        public string Password { get; set; } = string.Empty;
        [MinLength(5), MaxLength(20)]
        public string DisplayName { get; set; } = string.Empty;
    }
}
