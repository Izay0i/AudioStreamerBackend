using Microsoft.ML.Data;

namespace RecommendationSystem
{
    internal class RatingPrediction
    {
        [ColumnName("Score")]
        public float Rating;
    }
}
