using Microsoft.ML;
using Microsoft.ML.Data;
using System.Configuration;
using Npgsql;
using Newtonsoft.Json.Converters;

namespace RecommendationSystem
{
    internal class Program
    {
        static (IDataView trainingDataView, IDataView testDataView) LoadData(MLContext context)
        {
            var connectionString = ConfigurationManager.AppSettings["connectionString"];
            var loader = context.Data.CreateDatabaseLoader<UserStats>();
            var sqlCommand = "SELECT CAST(member_id as REAL) as member_id, CAST(track_id as REAL) as track_id, CAST(rating as REAL) as rating from memberstats";
            var dbSource = new DatabaseSource(NpgsqlFactory.Instance, connectionString, sqlCommand);
            Console.WriteLine("Loading data from database...");
            var data = loader.Load(dbSource);

            var set = context.Data.TrainTestSplit(data, testFraction: 0.3);
            return (set.TrainSet, set.TestSet);
        }

        static ITransformer Train(MLContext context, IDataView trainData)
        {
            var pipeline = context.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(UserStats.Rating))
                                                .Append(context.Transforms.Categorical.OneHotEncoding(outputColumnName: "MemberIdEncoded", inputColumnName: nameof(UserStats.MemberId)))
                                                .Append(context.Transforms.Categorical.OneHotEncoding(outputColumnName: "TrackIdEncoded", inputColumnName: nameof(UserStats.TrackId)))
                                                .Append(context.Transforms.Concatenate("Features", "MemberIdEncoded", "TrackIdEncoded"))
                                                .Append(context.Regression.Trainers.FastTree());
            var model = pipeline.Fit(trainData);
            return model;
        }

        static void Evaluate(MLContext context, ITransformer model, IDataView testData)
        {
            /*
             * The root of mean squared error (RMS or RMSE) is used to measure the differences between the model predicted values 
             * and the test dataset observed values. 
             * Technically it's the square root of the average of the squares of the errors. 
             * The lower it is, the better the model is.
             */

            /*
             * R Squared indicates how well data fits a model. 
             * Ranges from 0 to 1. 
             * A value of 0 means that the data is random or otherwise can't be fit to the model. 
             * A value of 1 means that the model exactly matches the data. 
             * You want your R Squared score to be as close to 1 as possible.
             */

            var predictions = model.Transform(testData);
            var metrics = context.Regression.Evaluate(predictions, "Label", "Score");
            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
        }

        static void TestSinglePrediction(MLContext context, ITransformer model)
        {
            var predictionFunction = context.Model.CreatePredictionEngine<UserStats, RatingPrediction>(model);
            var sample = new UserStats
            {
                MemberId = 10,
                TrackId = 53,
            };
            //No it's not accurate by all means, but I'll take what I can
            var prediction = predictionFunction.Predict(sample);
            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted rating: {prediction.Rating:0.####}");
            Console.WriteLine($"**********************************************************************");

            //Specify a hard limit whether or not if you want to suggest this item to the user
            var recommend = prediction.Rating >= 0.65 ? "Yes" : "No";
            Console.WriteLine($"Recommend track with id: {sample.TrackId} to user with id: {sample.MemberId}? {recommend}");
        }

        static void SaveModel(MLContext context, ITransformer model, DataViewSchema dataSchema, string modelPath)
        {
            //It's in /bin
            //Thanks
            //No problem
            Console.WriteLine("=============== Saving the model to a file ===============");
            var path = Path.Combine(Environment.CurrentDirectory, modelPath);
            context.Model.Save(model, dataSchema, path);
        }

        static void Main(string[] args)
        {
            //Jesse we need to predict
            //Hell yeah Mr. White
            var context = new MLContext();
            (IDataView trainData, IDataView testData) = LoadData(context);
            var model = Train(context, trainData);
            Evaluate(context, model, testData);
            TestSinglePrediction(context, model);
            SaveModel(context, model, trainData.Schema, "model.zip");
        }
    }
}