using AudioStreamerAPI.Constants;
using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public IEnumerable<Member> GetMembers() => MemberDAO.Instance.GetMembers();
        public IEnumerable<Member> SearchMembers(string keyword) => MemberDAO.Instance.SearchMembers(keyword);
        public Member? GetMember(int id) => MemberDAO.Instance.GetMember(id);
        public Member? GetMember(string email) => MemberDAO.Instance.GetMember(email);
        public OperationalStatus AddMember(Member member) => MemberDAO.Instance.AddMember(member);
        public OperationalStatus UpdateMember(Member member) => MemberDAO.Instance.UpdateMember(member);
        public OperationalStatus DeleteMember(string email) => MemberDAO.Instance.DeleteMember(email);
    }
}
