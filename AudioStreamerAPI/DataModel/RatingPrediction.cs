using Microsoft.ML.Data;

namespace AudioStreamerAPI.DataModel
{
    public class RatingPrediction
    {
        [ColumnName("Score")]
        public float Rating;
    }
}
