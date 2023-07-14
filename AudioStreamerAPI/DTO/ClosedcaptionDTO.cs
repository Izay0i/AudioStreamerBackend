using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioStreamerAPI.DTO
{
    public class ClosedcaptionDTO
    {
        [Key]
        public int CaptionId { get; set; }
        public int TrackId { get; set; }
        [DefaultValue("[]")]
        public string Captions { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
