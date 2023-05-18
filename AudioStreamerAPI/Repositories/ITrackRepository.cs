using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface ITrackRepository
    {
        IEnumerable<Track> GetTracks();
        IEnumerable<Track> SearchTracks(string keyword);
        Track? GetTrack(int id);
        Track? GetTrack(string name, string[]? tags);
        OperationalStatus AddTrack(Track track);
        OperationalStatus UpdateTrack(Track track);
        OperationalStatus DeleteTrack(int id);
    }
}
