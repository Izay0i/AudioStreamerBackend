using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _repo;

        public PlaylistController(IPlaylistRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
        {
            return await Task.FromResult(_repo.GetPlaylists().ToList());
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylistsFromUser(int id)
        {
            return await Task.FromResult(_repo.GetPlaylistsFromUser(id).ToList());
        }

        [HttpGet("user/playlist/{id}")]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracksFromPlaylist(int id)
        {
            return await Task.FromResult(_repo.GetTracksFromPlaylist(id).ToList());
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Playlist>>> SearchPlaylists(string name)
        {
            return await Task.FromResult(_repo.SearchPlaylists(name).ToList());
        }

        [HttpPost]
        public IActionResult AddPlaylist([FromBody] Playlist playlist)
        {
            return _repo.AddPlaylist(playlist) == OperationalStatus.SUCCESS ? Ok(playlist) : BadRequest(playlist);
        }

        [HttpPut]
        public IActionResult UpdatePlaylist([FromBody] Playlist playlist)
        {
            return _repo.UpdatePlaylist(playlist) == OperationalStatus.SUCCESS ? Ok(playlist) : NotFound(playlist);
        }

        [HttpDelete]
        public IActionResult DeletePlaylist(int id)
        {
            Playlist? playlist = _repo.GetPlaylist(id);
            if (playlist != null)
            {
                return _repo.DeletePlaylist(id) == OperationalStatus.SUCCESS ? Ok(id) : NotFound(id);
            }
            return NotFound(id);
        }

        [HttpPut("track/add/{id}/{trackId}")]
        public IActionResult AddTrack(int id, int trackId)
        {
            return _repo.AddTrack(id, trackId) == OperationalStatus.SUCCESS ? Ok() : NotFound(new object[] { id, trackId });
        }

        [HttpPut("track/remove/{id}/{trackId}")]
        public IActionResult Remove(int id, int trackId)
        {
            return _repo.RemoveTrack(id, trackId) == OperationalStatus.SUCCESS ? Ok() : NotFound(new object[] { id, trackId });
        }
    }
}
