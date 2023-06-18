using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface ITrackRepository
    {
        IEnumerable<Track> GetTracks();
        IEnumerable<Track> GetTracksWithTheMostViewsOfTheDay();
        IEnumerable<Track> GetTracksFromUserId(int uId);
        IEnumerable<Track> SearchTracks(string keyword);
        Track? GetTrack(int id);
        Track? GetTrack(string name, string[]? tags);
        OperationalStatus AddTrack(Track track);
        OperationalStatus UpdateTrack(Track track);
        //The names are getting longer while the definitions are getting shorter
        //Oh hey same length, nice
        OperationalStatus IncreaseViewCountsOfTheDay(int tId);
        OperationalStatus ResetViewCountsOfAllTracks();
        OperationalStatus DeleteTrack(int id);
    }
}
