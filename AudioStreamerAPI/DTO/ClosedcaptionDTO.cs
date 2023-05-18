namespace AudioStreamerAPI.DTO
{
    public class ClosedcaptionDTO
    {
        public int CaptionId { get; set; }
        public int TrackId { get; set; }
        public string Captions { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
