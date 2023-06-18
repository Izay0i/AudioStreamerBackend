using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackRepository _repo;
        private readonly IMapper _mapper;

        public TrackController(ITrackRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> GetTracks()
        {
            var tracks = _mapper.Map<IEnumerable<TrackDTO>>(_repo.GetTracks());
            return await Task.FromResult(tracks.ToList());
        }

        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> GetTracksWithTheMostViewsOfTheDay()
        {
            var tracks = _mapper.Map<IEnumerable<TrackDTO>>(_repo.GetTracksWithTheMostViewsOfTheDay());
            return await Task.FromResult(tracks.ToList());
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> GetTracksFromUserId(int uId)
        {
            var tracks = _mapper.Map<IEnumerable<TrackDTO>>(_repo.GetTracksFromUserId(uId));
            return await Task.FromResult(tracks.ToList());
        }

        [HttpGet("id/{id}")]
        public IActionResult GetTrack(int id)
        {
            TrackDTO? track = _mapper.Map<TrackDTO>(_repo.GetTrack(id));
            var result = track != null ? new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = $"Found track with id: ${id}.",
                Objects = new object[] { track },
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = $"Track with id: ${id} not found.",
            };
            //return track == null ? NotFound(id) : Ok(track);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{keyword}")]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> SearchTracks(string keyword)
        {
            var tracks = _mapper.Map<IEnumerable<TrackDTO>>(_repo.SearchTracks(keyword));
            return await Task.FromResult(tracks.ToList());
        }

        [HttpPost]
        public IActionResult AddTrack([FromBody] TrackDTO trackDTO)
        {
            var track = _mapper.Map<Track>(trackDTO);
            var result = _repo.AddTrack(track);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public IActionResult UpdateTrackInfo([FromBody] TrackDTO trackDTO)
        {
            var track = _mapper.Map<Track>(trackDTO);
            var result = _repo.UpdateTrack(track);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("incviews/track/{id}")]
        public IActionResult IncreaseViewCountsOfTheDay(int id)
        {
            var result = _repo.IncreaseViewCountsOfTheDay(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("resetviews")]
        public IActionResult ResetViewCountsOfAllTracks()
        {
            var result = _repo.ResetViewCountsOfAllTracks();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public IActionResult DeleteTrack(int id)
        {
            var result = _repo.DeleteTrack(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
