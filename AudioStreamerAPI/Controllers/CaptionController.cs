using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaptionController : ControllerBase
    {
        private readonly ICaptionRepository _repo;
        private readonly IMapper _mapper;

        public CaptionController(ICaptionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClosedcaptionDTO>>> GetClosedcaptions()
        {
            var captions = _mapper.Map<IEnumerable<ClosedcaptionDTO>>(_repo.GetClosedcaptions());
            return await Task.FromResult(captions.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetClosedcaption(int id)
        {
            ClosedcaptionDTO? closedcaption = _mapper.Map<ClosedcaptionDTO>(_repo.GetClosedcaption(id));
            var result = closedcaption != null ? new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = $"Successfully retrieved captions with id: {id}.",
                Objects = new object[] { closedcaption },
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = $"Failed to retrieve captions with id: {id}.",
            };
            return StatusCode((int)result.StatusCode, result);
        }

        //Man, I love auto-generated code
        [HttpGet("track/{id}")]
        public IActionResult GetClosedcaptionFromTrackId(int id)
        {
            ClosedcaptionDTO? closedcaption = _mapper.Map<ClosedcaptionDTO>(_repo.GetClosedcaptionFromTrackId(id));
            var result = closedcaption != null ? new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = $"Successfully retrieved captions for track with id: {id}.",
                Objects = new object[] { closedcaption },
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = $"Failed to retrieve captions for track with id: {id}.",
            };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public IActionResult AddClosedcaption([FromBody] ClosedcaptionDTO closedcaptionDTO)
        {
            var caption = _mapper.Map<Closedcaption>(closedcaptionDTO);
            var result = _repo.AddClosedcaption(caption);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public IActionResult UpdateClosedcaption([FromBody] ClosedcaptionDTO closedcaptionDTO)
        {
            var caption = _mapper.Map<Closedcaption>(closedcaptionDTO);
            var result = _repo.UpdateClosedcaption(caption);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public IActionResult DeleteClosedcaption(int id)
        {
            var result = _repo.DeleteClosedcaption(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
