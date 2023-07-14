using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AutoMapper;

namespace AudioStreamerAPI.Profiles
{
    public class ArtistInfoProfile : Profile
    {
        public ArtistInfoProfile()
        {
            CreateMap<Artistinfo, ArtistInfoDTO>()
                .ReverseMap();
        }
    }
}
