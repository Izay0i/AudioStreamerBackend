using Microsoft.AspNetCore.Mvc;
using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.DTO;
using AutoMapper;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberstatsController : ControllerBase
    {
        private readonly IMemberstatsRepository _repo;
        private readonly ITrackRepository _trackRepo;
        private readonly IMapper _mapper;

        public MemberstatsController(IMemberstatsRepository repo, ITrackRepository trackRepository, IMapper mapper)
        {
            _repo = repo;
            _trackRepo = trackRepository;
            _mapper = mapper;
        }

        [HttpGet("top/{limit}/genre/{gId}")]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> GetTopTracksWithMostViewsAndLikesFromGenre(int limit, int gId)
        {
            var results = _repo.GetCustomStatsOfEachTrackWithMostViewsAndLikesFromGenre(gId, limit);
            List<TrackDTO> tracks = new();
            foreach (var result in results)
            {
                var track = _mapper.Map<TrackDTO>(_trackRepo.GetTrack(result.trackId));
                if (track != null)
                {
                    tracks.Add(track);
                }
            }
            return await Task.FromResult(tracks);
        }

        [HttpGet("track/{id}")]
        public IActionResult GetStats(int id)
        {
            var result = _repo.GetStats(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("user/{uId}/track/{tId}")]
        public IActionResult GetStats(int uId, int tId)
        {
            MemberstatsDTO? stats = _mapper.Map<MemberstatsDTO>(_repo.GetStats(uId, tId));
            var result = new OperationalStatus
            {
                StatusCode = stats != null ? Constants.OperationalStatusEnums.Ok : Constants.OperationalStatusEnums.NotFound,
                Message = stats != null ? $"Found stats for (uId, tId): {uId}, {tId}." : "Stats not found.",
                Objects = stats != null ? new object[] { stats } : null,
            };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public IActionResult AddStats(MemberstatsDTO memberstats)
        {
            var stats = _mapper.Map<Memberstat>(memberstats);
            var result = _repo.AddStats(stats);
            var obj = _mapper.Map<MemberstatsDTO>(result.Objects?[0]);
            result.Objects = new object[] { obj };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public IActionResult UpdateStats(MemberstatsDTO memberstats)
        {
            var stats = _mapper.Map<Memberstat>(memberstats);
            var result = _repo.UpdateStats(stats);
            var obj = _mapper.Map<MemberstatsDTO>(result.Objects?[0]);
            result.Objects = new object[] { obj };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public IActionResult DeleteStats(int userId, int trackId)
        {
            var result = _repo.DeleteStats(userId, trackId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("user/{id}")]
        public IActionResult DeleteStatsOfUser(int id)
        {
            var result = _repo.DeleteStatsOfUser(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("track/{id}")]
        public IActionResult DeleteStatsOfTrack(int id)
        {
            var result = _repo.DeleteStatsOfTrack(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("track/{tId}/genre/{gId}")]
        public IActionResult ChangeGenreOfTrack(int tId, int gId)
        {
            var result = _repo.ChangeGenreOfTrack(tId, gId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
