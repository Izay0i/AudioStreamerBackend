using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Models;
using AutoMapper;

namespace AudioStreamerAPI.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreDTO>()
                .ReverseMap();
        }
    }
}
