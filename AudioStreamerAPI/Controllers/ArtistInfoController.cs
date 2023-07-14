using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistInfoController : ControllerBase
    {
        private readonly IArtistInfoRepository _repo;
        private readonly IMapper _mapper;

        public ArtistInfoController(IArtistInfoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistInfoDTO>>> GetArtists()
        {
            var artists = _mapper.Map<IEnumerable<ArtistInfoDTO>>(_repo.GetArtists());
            return await Task.FromResult(artists.ToList());
        }

        [HttpGet("tracks")]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> GetTracks(int id)
        {
            var tracks = _mapper.Map<IEnumerable<TrackDTO>>(_repo.GetTracks(id));
            return await Task.FromResult(tracks.ToList());
        }

        [HttpGet("id/{id}")]
        public IActionResult GetArtist(int id)
        {
            var artistDTO = _mapper.Map<ArtistInfoDTO>(_repo.GetArtist(id));
            var result = artistDTO != null ? new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = "Found artist.",
                Objects = new object[] { artistDTO },
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Artist not found.",
            };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetArtist(string name)
        {
            var artistDTO = _mapper.Map<ArtistInfoDTO>(_repo.GetArtist(name));
            var result = artistDTO != null ? new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = "Found artist.",
                Objects = new object[] { artistDTO },
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Artist not found.",
            };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("name")]
        public IActionResult GetArtistName(int id)
        {
            var name = _repo.GetArtistName(id);
            return Ok(new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = "Wanna know the name? Devil May Cry",
                Objects = new object[] { name },
            });
        }

        [HttpPost]
        public IActionResult AddArtist(ArtistInfoDTO artistDTO)
        {
            var artist = _mapper.Map<Artistinfo>(artistDTO);
            var result = _repo.AddArtist(artist);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public IActionResult UpdateArtist(ArtistInfoDTO artistDTO)
        {
            var artist = _mapper.Map<Artistinfo>(artistDTO);
            var result = _repo.UpdateArtist(artist);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public IActionResult DeleteArtist(int id)
        {
            var result = _repo.DeleteArtist(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
