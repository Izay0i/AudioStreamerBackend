using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        public IEnumerable<Track> GetTracks() => TrackDAO.Instance.GetTracks();
        public IEnumerable<Track> GetTracksWithTheMostViewsOfTheDay(int n) => TrackDAO.Instance.GetTracksWithTheMostViewsOfTheDay(n);
        public IEnumerable<Track> GetTracksFromUserId(int uId) => TrackDAO.Instance.GetTracksFromUserId(uId);
        public IEnumerable<Track> SearchTracks(string keyword) => TrackDAO.Instance.SearchTracks(keyword);
        public IEnumerable<int> GetRandomTrackIds(int limit) => TrackDAO.Instance.GetRandomTrackIds(limit);
        public Track? GetTrack(int id) => TrackDAO.Instance.GetTrack(id);
        public Track? GetTrack(string name, string[]? tags) => TrackDAO.Instance.GetTrack(name, tags);
        public OperationalStatus AddTrack(Track track) => TrackDAO.Instance.AddTrack(track);
        public OperationalStatus UpdateTrack(Track track) => TrackDAO.Instance.UpdateTrack(track);
        public OperationalStatus IncreaseViewCountsOfTheDay(int tId) => TrackDAO.Instance.IncreaseViewCountsOfTheDay(tId);
        public OperationalStatus ResetViewCountsOfAllTracks() => TrackDAO.Instance.ResetViewCountsOfAllTracks();
        public OperationalStatus DeleteTrack(int id) => TrackDAO.Instance.DeleteTrack(id);
    }
}
