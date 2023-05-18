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
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut]
        public IActionResult UpdateTrackInfo([FromBody] TrackDTO trackDTO)
        {
            var track = _mapper.Map<Track>(trackDTO);
            var result = _repo.UpdateTrack(track);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpDelete]
        public IActionResult DeleteTrack(int id)
        {
            var result = _repo.DeleteTrack(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
