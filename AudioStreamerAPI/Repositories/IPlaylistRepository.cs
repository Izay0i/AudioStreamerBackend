using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface IPlaylistRepository
    {
        //I really need to document my code cuz this is getting confusing
        //Yep, getting brain damage
        IEnumerable<Playlist> GetPlaylists();
        IEnumerable<Playlist> GetPlaylistsFromUser(int id);
        IEnumerable<Track> GetTracksFromPlaylist(int id);
        IEnumerable<Playlist> SearchPlaylists(string name);
        Playlist? GetPlaylist(int id);
        Playlist? GetPlaylistFromUser(int userId, int playlistId);
        Playlist? GetPlaylistFromUser(int userId, string name);
        IEnumerable<Playlist> GetPlaylists(string name);
        OperationalStatus AddPlaylist(Playlist playlist);
        OperationalStatus UpdatePlaylist(Playlist playlist);
        OperationalStatus DeletePlaylist(int id);
        OperationalStatus AddTrack(int id, int trackId);
        OperationalStatus RemoveTrack(int id, int trackId);
    }
}
