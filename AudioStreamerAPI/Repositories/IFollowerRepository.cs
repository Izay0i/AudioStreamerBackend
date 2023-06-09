﻿using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface IFollowerRepository
    {
        IEnumerable<Member> GetFollowingsFromUser(int id);
        IEnumerable<int> GetFollowingIdsFromUser(int id);
        OperationalStatus FollowMember(int id, int followingId);
        OperationalStatus UnfollowMember(int id, int followingId);
    }
}
