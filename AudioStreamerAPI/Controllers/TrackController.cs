using AudioStreamerAPI.Constants;
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
            if (_repo.AddTrack(track) == OperationalStatus.SUCCESS)
            {
                return Ok(track);
            }
            return BadRequest(track);
        }

        [HttpPut]
        public IActionResult UpdateTrackInfo([FromBody] TrackDTO trackDTO)
        {
            var track = _mapper.Map<Track>(trackDTO);
            if (_repo.UpdateTrack(track) == OperationalStatus.SUCCESS)
            {
                return Ok(track);
            }
            return NotFound(track.TrackName);
        }

        [HttpDelete]
        public IActionResult DeleteTrack(int id)
        {
            if (_repo.DeleteTrack(id) == OperationalStatus.SUCCESS)
            {
                return Ok();
            }
            return NotFound(id);
        }
    }
}
