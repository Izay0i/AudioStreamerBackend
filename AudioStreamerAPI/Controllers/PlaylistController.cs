using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AudioStreamerAPI.DTO;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _repo;
        private readonly IMapper _mapper;

        public PlaylistController(IPlaylistRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlaylists()
        {
            var playlists = _mapper.Map<IEnumerable<PlaylistDTO>>(_repo.GetPlaylists());
            return await Task.FromResult(playlists.ToList());
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlaylistsFromUser(int id)
        {
            var playlists = _mapper.Map<IEnumerable<PlaylistDTO>>(_repo.GetPlaylistsFromUser(id));
            return await Task.FromResult(playlists.ToList());
        }

        [HttpGet("user/playlist/{id}")]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> GetTracksFromPlaylist(int id)
        {
            var tracks = _mapper.Map<IEnumerable<TrackDTO>>(_repo.GetTracksFromPlaylist(id));
            return await Task.FromResult(tracks.ToList());
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> SearchPlaylists(string name)
        {
            var playlists = _mapper.Map<IEnumerable<PlaylistDTO>>(_repo.SearchPlaylists(name));
            return await Task.FromResult(playlists.ToList());
        }

        [HttpPost]
        public IActionResult AddPlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            var playlist = _mapper.Map<Playlist>(playlistDTO);
            var result = _repo.AddPlaylist(playlist);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut]
        public IActionResult UpdatePlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            var playlist = _mapper.Map<Playlist>(playlistDTO);
            var result = _repo.UpdatePlaylist(playlist);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpDelete]
        public IActionResult DeletePlaylist(int id)
        {
            Playlist? playlist = _repo.GetPlaylist(id);
            if (playlist != null)
            {
                var result = _repo.DeletePlaylist(id);
                return StatusCode((int)result.StatusCode, result.Message);
            }
            return NotFound(id);
        }

        [HttpPut("playlist/{id}/add/track/{trackId}")]
        public IActionResult AddTrack(int id, int trackId)
        {
            var result = _repo.AddTrack(id, trackId);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut("playlist/{id}/remove/track/{trackId}")]
        public IActionResult Remove(int id, int trackId)
        {
            var result = _repo.RemoveTrack(id, trackId);
            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
