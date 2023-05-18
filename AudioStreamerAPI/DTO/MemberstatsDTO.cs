using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public class MemberstatsDTO
    {
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int TrackId { get; set; }
        public int ViewCountsTotal { get; set; }
        public int Rating { get; set; }
        public string[]? Tags { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
