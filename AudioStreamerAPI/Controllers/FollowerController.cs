using AudioStreamerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Constants;
using AutoMapper;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowerController : ControllerBase
    {
        private readonly IFollowerRepository _repo;
        private readonly IMapper _mapper;

        public FollowerController(IFollowerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("user/{id}")]
        public async Task<IEnumerable<MemberDTO>> GetFollowingsFromUser(int id)
        {
            var members = _mapper.Map<IEnumerable<MemberDTO>>(_repo.GetFollowingsFromUser(id));
            return await Task.FromResult(members.ToList());
        }

        [HttpPost("user/{id}/follow/{followingId}")]
        public IActionResult FollowMember(int id, int followingId)
        {
            var result = _repo.FollowMember(id, followingId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("user/{id}/unfollow/{followingId}")]
        public IActionResult UnfollowMember(int id, int followingId)
        {
            var result = _repo.UnfollowMember(id, followingId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
