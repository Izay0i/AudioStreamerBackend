using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class ArtistInfoRepository : IArtistInfoRepository
    {
        public IEnumerable<Artistinfo> GetArtists() => ArtistInfoDAO.Instance.GetArtists();
        public IEnumerable<Track> GetTracks(int id) => ArtistInfoDAO.Instance.GetTracks(id);
        public Artistinfo? GetArtist(int id) => ArtistInfoDAO.Instance.GetArtist(id);
        public Artistinfo? GetArtist(string name) => ArtistInfoDAO.Instance.GetArtist(name);
        public string GetArtistName(int id) => ArtistInfoDAO.Instance.GetArtistName(id);
        public OperationalStatus AddArtist(Artistinfo artistinfo) => ArtistInfoDAO.Instance.AddArtist(artistinfo);
        public OperationalStatus UpdateArtist(Artistinfo artistinfo) => ArtistInfoDAO.Instance.UpdateArtist(artistinfo);
        public OperationalStatus DeleteArtist(int id) => ArtistInfoDAO.Instance.DeleteArtist(id);
    }
}
