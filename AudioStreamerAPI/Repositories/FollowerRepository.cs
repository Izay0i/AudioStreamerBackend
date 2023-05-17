using AudioStreamerAPI.Constants;

namespace AudioStreamerAPI.Repositories
{
    public class FollowerRepository : IFollowerRepository
    {
        public OperationalStatus FollowMember(int id, int followingId) => FollowerDAO.Instance.FollowMember(id, followingId);
        public OperationalStatus UnfollowMember(int id, int followingId) => FollowerDAO.Instance.UnfollowMember(id, followingId);
    }
}
