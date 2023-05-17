using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class FollowerDAO
    {
        private static FollowerDAO? _instance;
        private static readonly object _instanceLock = new();

        public static FollowerDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new();
                    }
                    return _instance;
                }
            }
        }

        public OperationalStatus FollowMember(int id, int followingId)
        {
            var member = MemberDAO.Instance.GetMember(id);
            var followingMember = MemberDAO.Instance.GetMember(followingId);

            if (member == null || followingMember == null)
            {
                return OperationalStatus.FAILURE;
            }

            if (member.FollowingIds!.Contains(followingId))
            {
                return OperationalStatus.FAILURE;
            }

            if (member.FollowingIds!.Length == 0)
            {
                member.FollowingIds = new int[] { followingId };
            }
            else
            {
                var followings = member.FollowingIds.ToList();
                followings.Add(followingId);

                var newArrayLength = followings.Count;
                member.FollowingIds = new int[newArrayLength];
                followings.CopyTo(member.FollowingIds, 0);
            }

            if (MemberDAO.Instance.UpdateMember(member) == OperationalStatus.SUCCESS)
            {
                return OperationalStatus.SUCCESS;
            }
            return OperationalStatus.FAILURE;
        }

        public OperationalStatus UnfollowMember(int id, int followingId)
        {
            //Déjà vu?
            var member = MemberDAO.Instance.GetMember(id);
            var followingMember = MemberDAO.Instance.GetMember(followingId);

            if (member == null || followingMember == null)
            {
                return OperationalStatus.FAILURE;
            }

            if (member.FollowingIds!.Length == 0)
            {
                return OperationalStatus.FAILURE;
            }
            else
            {
                var followings = member.FollowingIds.ToList();
                followings.Remove(followingId);

                var newArrayLength = followings.Count;
                member.FollowingIds = new int[newArrayLength];
                followings.CopyTo(member.FollowingIds, 0);
            }

            if (MemberDAO.Instance.UpdateMember(member) == OperationalStatus.SUCCESS)
            {
                return OperationalStatus.SUCCESS;
            }
            return OperationalStatus.FAILURE;
        }
    }
}
