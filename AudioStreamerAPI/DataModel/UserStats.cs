using Microsoft.ML.Data;

namespace AudioStreamerAPI.DataModel
{
    public class UserStats
    {
        public float MemberId { get; set; }
        public float TrackId { get; set; }
        public float Rating;
    }
}
