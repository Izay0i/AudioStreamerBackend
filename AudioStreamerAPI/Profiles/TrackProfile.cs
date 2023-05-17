using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AutoMapper;

namespace AudioStreamerAPI.Profiles
{
    public class TrackProfile : Profile
    {
        public TrackProfile() 
        {
            CreateMap<Track, TrackDTO>()
                .ReverseMap();
        }
    }
}
