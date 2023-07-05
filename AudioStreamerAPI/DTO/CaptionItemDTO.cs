namespace AudioStreamerAPI.DTO
{
    public class CaptionItemDTO
    {
        public double Timestamp { get; set; }
        public double Duration { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
