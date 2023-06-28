using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class FollowerRepository : IFollowerRepository
    {
        public IEnumerable<Member> GetFollowingsFromUser(int id) => FollowerDAO.Instance.GetFollowingsFromUser(id);
        public IEnumerable<int> GetFollowingIdsFromUser(int id) => FollowerDAO.Instance.GetFollowingIdsFromUser(id);
        public OperationalStatus FollowMember(int id, int followingId) => FollowerDAO.Instance.FollowMember(id, followingId);
        public OperationalStatus UnfollowMember(int id, int followingId) => FollowerDAO.Instance.UnfollowMember(id, followingId);
    }
}
