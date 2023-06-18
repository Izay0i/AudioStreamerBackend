using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        public IEnumerable<Playlist> GetPlaylists() => PlaylistDAO.Instance.GetPlaylists();
        public IEnumerable<Playlist> GetPlaylistsFromUser(int id) => PlaylistDAO.Instance.GetPlaylistsFromUser(id);
        public IEnumerable<Track> GetTracksFromPlaylist(int id) => PlaylistDAO.Instance.GetTracksFromPlaylist(id);
        public IEnumerable<Playlist> SearchPlaylists(string name) => PlaylistDAO.Instance.SearchPlaylists(name);
        public Playlist? GetPlaylist(int id) => PlaylistDAO.Instance.GetPlaylist(id);
        public Playlist? GetPlaylistFromUser(int userId, int playlistId) => PlaylistDAO.Instance.GetPlaylistFromUser(userId, playlistId);
        public Playlist? GetPlaylistFromUser(int userId, string name) => PlaylistDAO.Instance.GetPlaylistFromUser(userId, name);
        public IEnumerable<Playlist> GetPlaylists(string name) => PlaylistDAO.Instance.GetPlaylists(name);
        public OperationalStatus AddPlaylist(Playlist playlist) => PlaylistDAO.Instance.AddPlaylist(playlist);
        public OperationalStatus UpdatePlaylist(Playlist playlist) => PlaylistDAO.Instance.UpdatePlaylist(playlist);
        public OperationalStatus DeletePlaylist(int id) => PlaylistDAO.Instance.DeletePlaylist(id);
        public OperationalStatus AddTrack(int id, int trackId) => PlaylistDAO.Instance.AddTrack(id, trackId);
        public OperationalStatus RemoveTrack(int id, int trackId) => PlaylistDAO.Instance.RemoveTrack(id, trackId);
    }
}
