using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        IEnumerable<Track> ITrackRepository.GetTracks() => TrackDAO.Instance.GetTracks();
        IEnumerable<Track> ITrackRepository.SearchTracks(string keyword) => TrackDAO.Instance.SearchTracks(keyword);
        Track? ITrackRepository.GetTrack(int id) => TrackDAO.Instance.GetTrack(id);
        Track? ITrackRepository.GetTrack(string name, string[]? tags) => TrackDAO.Instance.GetTrack(name, tags);
        OperationalStatus ITrackRepository.AddTrack(Track track) => TrackDAO.Instance.AddTrack(track);
        OperationalStatus ITrackRepository.UpdateTrack(Track track) => TrackDAO.Instance.UpdateTrack(track);
        OperationalStatus ITrackRepository.DeleteTrack(int id) => TrackDAO.Instance.DeleteTrack(id);
    }
}
