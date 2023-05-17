using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowerController : ControllerBase
    {
        private readonly IFollowerRepository _repo;

        public FollowerController(IFollowerRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult FollowMember(int id, int followingId)
        {
            if (_repo.FollowMember(id, followingId) == OperationalStatus.SUCCESS)
            {
                return Ok();
            }
            return NotFound(new object[] { id, followingId });
        }

        [HttpDelete]
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
