using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackRepository _repo;

        public TrackController(ITrackRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
        {
            return await Task.FromResult(_repo.GetTracks().ToList());
        }

        [HttpGet("{keyword}")]
        public async Task<ActionResult<IEnumerable<Track>>> SearchTracks(string keyword)
        {
            return await Task.FromResult(_repo.SearchTracks(keyword).ToList());
        }

        [HttpPost]
        public IActionResult AddTrack([FromBody] Track track)
        {
            if (_repo.AddTrack(track) == OperationalStatus.SUCCESS)
            {
                return Ok(track);
            }
            return BadRequest(track);
        }

        [HttpPut]
        public IActionResult UpdateTrackInfo([FromBody] Track track)
        {
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
