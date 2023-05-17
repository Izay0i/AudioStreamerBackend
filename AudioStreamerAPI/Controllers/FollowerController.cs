using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.Constants;
using Microsoft.AspNetCore.Mvc;
using AudioStreamerAPI.DTO;
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
            if (_repo.FollowMember(id, followingId) == OperationalStatus.SUCCESS)
            {
                return Ok();
            }
            return NotFound(new object[] { id, followingId });
        }

        [HttpDelete("user/{id}/unfollow/{followingId}")]
        public IActionResult UnfollowMember(int id, int followingId)
        {
            if (_repo.UnfollowMember(id, followingId) == OperationalStatus.SUCCESS)
            {
                return Ok();
            }
            return NotFound(new object[] { id, followingId });
        }
    }
}
