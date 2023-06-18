using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public class MemberstatsDTO
    {
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int TrackId { get; set; }
        [DefaultValue(0)]
        public int ViewCountsTotal { get; set; }
        [DefaultValue(0)]
        public int Rating { get; set; }
        [DefaultValue(new string[0])]
        public string[]? Tags { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
