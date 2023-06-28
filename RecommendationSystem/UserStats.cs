using Microsoft.ML.Data;

namespace RecommendationSystem
{
    internal class UserStats
    {
        [LoadColumn(0)]
        public float MemberId { get; set; }
        [LoadColumn(1)]
        public float TrackId { get; set; }
        [LoadColumn(2)]
        public float Rating { get; set; }
    }
}
