using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AutoMapper;

namespace AudioStreamerAPI.Profiles
{
    public class PlaylistProfile : Profile
    {
        public PlaylistProfile() 
        { 
            CreateMap<Playlist, PlaylistDTO>()
                .ReverseMap();
        }
    }
}
