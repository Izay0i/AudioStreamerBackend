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
        private readonly IFollowerRepository _followerRepo;
        private readonly ITrackRepository _trackRepo;
        private readonly IMapper _mapper;

        public FollowerController(IFollowerRepository followerRepo, ITrackRepository trackRepo, IMapper mapper)
        {
            _followerRepo = followerRepo;
            _trackRepo = trackRepo;
            _mapper = mapper;
        }

        [HttpGet("user/{id}")]
        public async Task<IEnumerable<MemberDTO>> GetFollowingsFromUser(int id)
        {
            var members = _mapper.Map<IEnumerable<MemberDTO>>(_followerRepo.GetFollowingsFromUser(id));
            return await Task.FromResult(members.ToList());
        }

        [HttpGet("tracks/user/{id}")]
        public async Task<IEnumerable<TrackDTO>> GetTracksFromUsersThatYourUserIsFollowingGodThatsSoLong(int id)
        {
            var memberIds = _followerRepo.GetFollowingIdsFromUser(id);
            List<TrackDTO> tracks = new();
            foreach (var mId in memberIds)
            {
                var userTracks = _mapper.Map<List<TrackDTO>>(_trackRepo.GetTracksFromUserId(mId));
                tracks.AddRange(userTracks);
            }
            tracks.RemoveAll(item => item == null);
            return await Task.FromResult(tracks.OrderByDescending(track => track.TrackId).ToList());
        }

        [HttpPost("user/{id}/follow/{followingId}")]
        public IActionResult FollowMember(int id, int followingId)
        {
            var result = _followerRepo.FollowMember(id, followingId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("user/{id}/unfollow/{followingId}")]
        public IActionResult UnfollowMember(int id, int followingId)
        {
            var result = _followerRepo.UnfollowMember(id, followingId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
