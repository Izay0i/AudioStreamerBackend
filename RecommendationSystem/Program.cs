using Microsoft.ML;
using Microsoft.ML.Data;
using System.Configuration;
using Npgsql;
using Microsoft.ML.Trainers;

namespace RecommendationSystem
{
    internal class Program
    {
        static (IDataView trainingDataView, IDataView testDataView) LoadData(MLContext context)
        {
            var connectionString = ConfigurationManager.AppSettings["connectionString"];
            var loader = context.Data.CreateDatabaseLoader<UserStats>();
            //var sqlCommand = "SELECT CAST(member_id as REAL) as member_id, CAST(track_id as REAL) as track_id, CAST(view_counts_total as REAL) as view_counts_total, CAST(rating as REAL) as rating from memberstats";
            var sqlCommand = "SELECT CAST(member_id as REAL) as member_id, CAST(track_id as REAL) as track_id, CAST(rating as REAL) as rating from memberstats";
            var dbSource = new DatabaseSource(NpgsqlFactory.Instance, connectionString, sqlCommand);
            Console.WriteLine("Loading data from database...");
            var data = loader.Load(dbSource);

            var set = context.Data.TrainTestSplit(data, testFraction: 0.3);
            return (set.TrainSet, set.TestSet);
        }

        static ITransformer TrainModel(MLContext context, IDataView trainData)
        {
            var targetMap = new Dictionary<float, bool>
            {
                { 1.0f, true },
                { 0.0f, false }
            };
            
            var a = context.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair(nameof(UserStats.MemberId), nameof(UserStats.MemberId)) });
            var b = context.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair(nameof(UserStats.TrackId), nameof(UserStats.TrackId)) });
            var c = context.Transforms.Concatenate("Features", new[] { nameof(UserStats.MemberId), nameof(UserStats.TrackId) });
            var dataPipe = context.Transforms.Conversion.MapValue(nameof(UserStats.Rating), targetMap)
                                    .Append(a)
                                    .Append(b)
                                    .Append(c);

            var options = new LbfgsLogisticRegressionBinaryTrainer.Options()
            {
                LabelColumnName = nameof(UserStats.Rating),
                FeatureColumnName = "Features",
                MaximumNumberOfIterations = 100,
                OptimizationTolerance = 1e-8f,
            };

            var trainer = context.BinaryClassification.Trainers.LbfgsLogisticRegression(options);
            var trainPipe = dataPipe.Append(trainer);
            ITransformer model = trainPipe.Fit(trainData);
            return model;
        }

        static void Evaluate(MLContext context, IDataView trainData, ITransformer model)
        {
            IDataView predictions = model.Transform(trainData);
            var metrics = context.BinaryClassification.EvaluateNonCalibrated(predictions, nameof(UserStats.Rating), "Score");
            Console.WriteLine($"Model accuracy: {metrics.Accuracy:F4}");

            var sample = new UserStats
            {
                MemberId = 10,
                TrackId = 53,
            };

            var pe = context.Model.CreatePredictionEngine<UserStats, RatingPrediction>(model);
            var output = pe.Predict(sample);
            Console.WriteLine($"Predicted label: {output.PredictedLabel}");
            Console.WriteLine($"Score: {output.Rating}");
        }

        static void SaveModel(MLContext context, ITransformer model, DataViewSchema dataSchema, string modelPath)
        {
            //It's in /bin
            //Thanks
            //No problem
            Console.WriteLine("Saving the model to a file");
            var path = Path.Combine(Environment.CurrentDirectory, modelPath);
            context.Model.Save(model, dataSchema, path);
        }

        static void Main(string[] args)
        {
            //Jesse we need to predict
            //Hell yeah Mr. White
            var context = new MLContext();
            (IDataView trainData, IDataView testData) = LoadData(context);
            ITransformer model = TrainModel(context, trainData);
            Evaluate(context, trainData, model);
            SaveModel(context, model, trainData.Schema, "model.zip");
        }
    }
}