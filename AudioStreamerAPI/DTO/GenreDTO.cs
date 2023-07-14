using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public class GenreDTO
    {
        [Key]
        public int GenreId { get; set; }
        [Required]
        public string GenreName { get; set; } = null!;
    }
}
