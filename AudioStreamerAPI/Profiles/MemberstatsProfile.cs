using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AutoMapper;

namespace AudioStreamerAPI.Profiles
{
    public class MemberstatsProfile : Profile
    {
        public MemberstatsProfile() 
        {
            CreateMap<Memberstat, MemberstatsDTO>()
                .ReverseMap();
        }
    }
}
