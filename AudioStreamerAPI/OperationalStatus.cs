using AudioStreamerAPI.Constants;

namespace AudioStreamerAPI
{
    public class OperationalStatus
    {
        public OperationalStatusEnums StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object[]? Objects { get; set; } = null;
    }
}
