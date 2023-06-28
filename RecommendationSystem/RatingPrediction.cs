using Microsoft.ML.Data;

namespace RecommendationSystem
{
    internal class RatingPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        [ColumnName("Score")]
        public float Rating { get; set; }
    }
}
