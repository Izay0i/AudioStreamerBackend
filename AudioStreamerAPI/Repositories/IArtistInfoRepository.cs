using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface IArtistInfoRepository
    {
        IEnumerable<Artistinfo> GetArtists();
        IEnumerable<Track> GetTracks(int id);
        Artistinfo? GetArtist(int id);
        Artistinfo? GetArtist(string name);
        string GetArtistName(int id);
        OperationalStatus AddArtist(Artistinfo artistinfo);
        OperationalStatus UpdateArtist(Artistinfo artistinfo);
        OperationalStatus DeleteArtist(int id);
    }
}
