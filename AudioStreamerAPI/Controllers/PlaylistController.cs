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

        [HttpGet("user/{uId}/playlists/{name}")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlaylistsFromUser(int uId, string name)
        {
            var playlists = _mapper.Map<IEnumerable<PlaylistDTO>>(_repo.GetPlaylistsFromUser(uId, name));
            return await Task.FromResult(playlists.ToList());
        }

        [HttpGet("tracks/{id}")]
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

        [HttpGet("user/{uId}/playlist/id/{pId}")]
        public IActionResult GetPlaylistByIdFromUser(int uId, int pId)
        {
            PlaylistDTO? playlistDTO = _mapper.Map<PlaylistDTO>(_repo.GetPlaylistByIdFromUser(uId, pId));
            var result = playlistDTO != null ? new OperationalStatus
            {
                //I don't believe in consistency
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = $"Found playlist with id: {pId}.",
                Objects = new object[] { playlistDTO }
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Playlist not found.",
            };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("user/{uId}/playlist/name/{name}")]
        public IActionResult GetPlaylistByNameFromUser(int uId, string name)
        {
            PlaylistDTO? playlistDTO = _mapper.Map<PlaylistDTO>(_repo.GetPlaylistByNameFromUser(uId, name));
            var result = playlistDTO != null ? new OperationalStatus
            {
                //I don't believe in consistency
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = $"Found playlist with name: {name}.",
                Objects = new object[] { playlistDTO }
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Playlist not found.",
            };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public IActionResult AddPlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            var playlist = _mapper.Map<Playlist>(playlistDTO);
            var result = _repo.AddPlaylist(playlist);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public IActionResult UpdatePlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            var playlist = _mapper.Map<Playlist>(playlistDTO);
            var result = _repo.UpdatePlaylist(playlist);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public IActionResult DeletePlaylist(int id)
        {
            var result = _repo.DeletePlaylist(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("{id}/add/track/{trackId}")]
        public IActionResult AddTrackToPlaylist(int id, int trackId)
        {
            var result = _repo.AddTrack(id, trackId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("{id}/remove/track/{trackId}")]
        public IActionResult RemoveTrackFromPlaylist(int id, int trackId)
        {
            var result = _repo.RemoveTrack(id, trackId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
