using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Helpers;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class MemberDAO
    {
        private static MemberDAO? _instance;
        private static readonly object _instanceLock = new();

        public static MemberDAO Instance
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

        public IEnumerable<Member> GetMembers()
        {
            List<Member>? members;
            try
            {
                var context = new fsnvdezgContext();
                members = context.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public IEnumerable<Member> SearchMembers(string keyword)
        {
            List<Member>? members;
            try
            {
                var context = new fsnvdezgContext();
                var filteredMembers = new List<Member>();
                filteredMembers.AddRange(context.Members.Where(m => m.DisplayName.Contains(keyword.Trim())).ToList());
                filteredMembers.AddRange(context.Members.Where(m => m.NameTag.Contains(keyword.Trim())).ToList());

                members = filteredMembers.Distinct().ToList();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public Member? GetMember(int id)
        {
            Member? member;
            try
            {
                var context = new fsnvdezgContext();
                member = context.Members.SingleOrDefault(m => m.MemberId.Equals(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public Member? GetMember(string email)
        {
            Member? member;
            try
            {
                var context = new fsnvdezgContext();
                member = context.Members.SingleOrDefault(m => m.Email.Equals(email.Trim()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public OperationalStatus AddMember(Member member)
        {
            Member? memberHasEmail = GetMember(member.Email);
            if (memberHasEmail != null)
            {
                return OperationalStatus.FAILURE;
            }
            else
            {
                try
                {
                    var context = new fsnvdezgContext();
                    Member m = new()
                    {
                        Email = member.Email,
                        Password = CredentialsHelper.HashPassword(member.Password),
                        Token = string.Empty,
                        DisplayName = member.DisplayName,
                        NameTag = member.DisplayName.ToLower().Trim().Replace(" ", ""),
                    };

                    context.Members.Add(m);
                    context.SaveChanges();
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public OperationalStatus UpdateMember(Member member)
        {
            Member? memberHasEmail = GetMember(member.Email);
            if (memberHasEmail != null)
            {
                /*if (CredentialsHelper.VerifyPassword(memberHasEmail.Password, member.Password) == OperationalStatus.FAILURE) 
                {
                    return OperationalStatus.FAILURE;
                }*/

                try
                {
                    var context = new fsnvdezgContext();
                    context.Members.Attach(memberHasEmail);

                    if (member.DisplayName != null)
                    {
                        memberHasEmail.DisplayName = member.DisplayName;
                    }

                    if (member.Biography != null)
                    {
                        memberHasEmail.Biography = member.Biography;
                    }

                    if (member.Avatar != null)
                    {
                        memberHasEmail.Avatar = member.Avatar;
                    }

                    if (member.FollowingIds != null)
                    {
                        var newArrayLength = member.FollowingIds.Length;
                        memberHasEmail.FollowingIds = new int[newArrayLength];
                        member.FollowingIds.CopyTo(memberHasEmail.FollowingIds, 0);
                    }

                    context.SaveChanges();
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return OperationalStatus.FAILURE;
            }
        }

        public OperationalStatus DeleteMember(string email)
        {
            Member? memberHasEmail = GetMember(email);
            if (memberHasEmail != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Members.Remove(memberHasEmail);
                    context.SaveChanges();
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return OperationalStatus.FAILURE;
            }
        }
    }
}
