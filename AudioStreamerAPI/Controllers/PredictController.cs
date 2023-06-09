﻿using AudioStreamerAPI.DataModel;
using AudioStreamerAPI.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.DTO;
using AutoMapper;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictController : ControllerBase
    {
        //https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/end-to-end-apps/Recommendation-MovieRecommender/MovieRecommender/movierecommender/Controllers/MoviesController.cs

        private readonly ITrackRepository _repo;
        private readonly IMapper _mapper;
        private readonly PredictionEnginePool<UserStats, RatingPrediction> _pool;

        public PredictController(ITrackRepository repo, IMapper mapper, PredictionEnginePool<UserStats, RatingPrediction> pool)
        {
            _repo = repo;
            _mapper = mapper;
            _pool = pool;
        }

        //Would not recommend
        [HttpGet]
        public ActionResult GetRecommended(int uId, int limit = NumericConstants.MAX_RECOMMENDED_ITEMS)
        {
            List<int> recommendedIds = new();
            List<TrackDTO> recommendedItems = new();
            int currentTries = 0;
            int numberOfTries = limit / 2;
            //To prevent infinite loop, hopefully
            while (recommendedIds.Count < limit && currentTries < numberOfTries)
            {
                var trackIds = _repo.GetRandomTrackIds(limit);
                foreach (var id in trackIds)
                {
                    var recommend = _pool.Predict(NamedConstants.PREDICTION_MODEL_NAME, new UserStats
                    {
                        MemberId = uId,
                        TrackId = id,
                    }).PredictedLabel;

                    if (recommend)
                    {
                        recommendedIds.Add(id);
                    }
                }
                recommendedIds = recommendedIds.Distinct().ToList();
                if (recommendedIds.Count < limit)
                {
                    ++currentTries;
                }
            }
            recommendedIds = recommendedIds
                .Take(limit)
                .OrderByDescending(id => id)
                .ToList();
            foreach (var trackId in recommendedIds)
            {
                var trackDTO = _mapper.Map<TrackDTO>(_repo.GetTrack(trackId));
                recommendedItems.Add(trackDTO);
            }
            return Ok(recommendedItems);
        }

        [HttpPost]
        public ActionResult Predict([FromBody] UserStats input)
        {
            var recommend = _pool.Predict(NamedConstants.PREDICTION_MODEL_NAME, input).PredictedLabel;
            var result = new OperationalStatus
            {
                StatusCode = recommend ? OperationalStatusEnums.Ok : OperationalStatusEnums.NotFound,
                Message = recommend ? "Recommended" : "Not recommended",
                Objects = new object[] { recommend }, 
            };
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
