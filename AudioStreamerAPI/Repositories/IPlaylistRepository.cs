using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface IPlaylistRepository
    {
        //I really need to document my code cuz this is getting confusing
        IEnumerable<Playlist> GetPlaylists();
        IEnumerable<Playlist> GetPlaylistsFromUser(int id);
        IEnumerable<Track> GetTracksFromPlaylist(int id);
        IEnumerable<Playlist> SearchPlaylists(string name);
        Playlist? GetPlaylist(int id);
        Playlist? GetPlaylist(string name);
        OperationalStatus AddPlaylist(Playlist playlist);
        OperationalStatus UpdatePlaylist(Playlist playlist);
        OperationalStatus DeletePlaylist(int id);
        OperationalStatus AddTrack(int id, int trackId);
        OperationalStatus RemoveTrack(int id, int trackId);
    }
}
