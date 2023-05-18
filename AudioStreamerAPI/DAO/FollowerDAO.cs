using AudioStreamerAPI.Models;
using AudioStreamerAPI.Constants;

namespace AudioStreamerAPI.DAO
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

        public IEnumerable<Member> GetFollowingsFromUser(int id)
        {
            List<Member> followings = new();
            var member = MemberDAO.Instance.GetMember(id);
            if (member != null && member.FollowingIds!.Length != 0)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    var filteredList = new List<Member?>();
                    foreach (var memberId in member.FollowingIds)
                    {
                        filteredList.Add(context.Members.FirstOrDefault(m => m.MemberId == memberId));
                    }
                    followings = filteredList.Where(m => m != null).Distinct().ToList()!;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return followings;
        }

        public OperationalStatus FollowMember(int id, int followingId)
        {
            var member = MemberDAO.Instance.GetMember(id);
            var followingMember = MemberDAO.Instance.GetMember(followingId);

            if (member == null || followingMember == null)
            {
                return new OperationalStatus 
                { 
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Neither userId: {id} nor followingId: {followingId} could be found."
                };
            }

            if (member.FollowingIds!.Contains(followingId))
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Conflict,
                    Message = $"Already following user: {followingMember.DisplayName}.",
                };
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

            var result = MemberDAO.Instance.UpdateMember(member);
            if (result.StatusCode == OperationalStatusEnums.Ok)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = $"Successfully added user with Id: {followingId} to list.",
                };
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = $"Failed to add user with Id: {followingId} to list.",
            };
        }

        public OperationalStatus UnfollowMember(int id, int followingId)
        {
            //Déjà vu?
            var member = MemberDAO.Instance.GetMember(id);
            var followingMember = MemberDAO.Instance.GetMember(followingId);

            if (member == null || followingMember == null)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Neither userId: {id} nor followingId: {followingId} can be found."
                };
            }

            if (member.FollowingIds!.Length == 0)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"User isn't following anyone.",
                };
            }
            else
            {
                var followings = member.FollowingIds.ToList();
                followings.Remove(followingId);

                var newArrayLength = followings.Count;
                member.FollowingIds = new int[newArrayLength];
                followings.CopyTo(member.FollowingIds, 0);
            }

            var result = MemberDAO.Instance.UpdateMember(member);
            if (result.StatusCode == OperationalStatusEnums.Ok)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = $"Successfully removed user with Id: {followingId} from list.",
                };
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = $"Failed to remove user with Id: {followingId} from list.",
            };
        }
    }
}
