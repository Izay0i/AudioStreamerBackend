using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetMembers();
        IEnumerable<Member> SearchMembers(string keyword);
        Member? GetMember(int id);
        Member? GetMember(string email);
        OperationalStatus AddMember(Member member);
        OperationalStatus UpdateMember(Member member);
        OperationalStatus DeleteMember(string email);
    }
}
