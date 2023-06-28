using Microsoft.ML.Data;

namespace AudioStreamerAPI.DataModel
{
    public class RatingPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        [ColumnName("Score")]
        public float Rating { get; set; }
    }
}
