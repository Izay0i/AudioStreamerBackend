using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AutoMapper;

namespace AudioStreamerAPI.Profiles
{
    public class CaptionProfile : Profile
    {
        public CaptionProfile() 
        { 
            CreateMap<Closedcaption, ClosedcaptionDTO>()
                .ReverseMap();
        }
    }
}
