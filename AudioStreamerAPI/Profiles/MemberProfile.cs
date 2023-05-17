using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AutoMapper;

namespace AudioStreamerAPI.Profiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile() 
        {
            CreateMap<Member, MemberDTO>()
                .ReverseMap();
        }
    }
}
