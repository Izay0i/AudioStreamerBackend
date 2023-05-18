using AudioStreamerAPI.Helpers;
using AudioStreamerAPI.Models;
using AudioStreamerAPI.Constants;

namespace AudioStreamerAPI.DAO
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
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.BadRequest,
                    Message = "Invalid request. I demand that, under no circumstances, should you ever try that again.",
                };
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
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Created,
                        Message = $"Successfully registered member with {m.Email}.",
                    };
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
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = "Successfully updated member's info.",
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = "Couldn't find member.",
                };
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
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = $"Successfully deleted member with {email}.",
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = "Couldn't find member.",
                };
            }
        }
    }
}
