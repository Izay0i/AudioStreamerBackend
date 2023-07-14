using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _repo;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {
            var genres = _mapper.Map<IEnumerable<GenreDTO>>(_repo.GetGenres());
            return await Task.FromResult(genres.ToList());
        }

        [HttpPost]
        public IActionResult AddGenre(GenreDTO genreDTO)
        {
            var genre = _mapper.Map<Genre>(genreDTO);
            var result = _repo.AddGenre(genre);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public IActionResult UpdateGenre(GenreDTO genreDTO)
        {
            var genre = _mapper.Map<Genre>(genreDTO);
            var result = _repo.UpdateGenre(genre);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public IActionResult DeleteGenre(int id)
        {
            var result = _repo.DeleteGenre(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
