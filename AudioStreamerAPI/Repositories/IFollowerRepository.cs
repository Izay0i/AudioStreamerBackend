using AudioStreamerAPI.Constants;

namespace AudioStreamerAPI.Repositories
{
    public interface IFollowerRepository
    {
        OperationalStatus FollowMember(int id, int followingId);
        OperationalStatus UnfollowMember(int id, int followingId);
    }
}
