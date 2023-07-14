using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class MemberstatsRepository : IMemberstatsRepository
    {
        public IEnumerable<dynamic> GetCustomStatsOfEachTrackWithMostViewsAndLikesFromGenre(int genreId, int limit) => MemberstatsDAO.Instance.GetCustomStatsOfEachTrackWithMostViewsAndLikesFromGenre(genreId, limit);
        public OperationalStatus GetStats(int trackId) => MemberstatsDAO.Instance.GetStats(trackId);
        public Memberstat? GetStats(int userId, int trackId) => MemberstatsDAO.Instance.GetStats(userId, trackId);
        public OperationalStatus AddStats(Memberstat memberstat) => MemberstatsDAO.Instance.AddStats(memberstat);
        public OperationalStatus UpdateStats(Memberstat memberstat) => MemberstatsDAO.Instance.UpdateStats(memberstat);
        public OperationalStatus DeleteStats(int userId, int trackId) => MemberstatsDAO.Instance.DeleteStats(userId, trackId);
        public OperationalStatus DeleteStatsOfUser(int userId) => MemberstatsDAO.Instance.DeleteStatsOfUser(userId);
        public OperationalStatus DeleteStatsOfTrack(int trackId) => MemberstatsDAO.Instance.DeleteStatsOfTrack(trackId);
        public OperationalStatus ChangeGenreOfTrack(int trackId, int genreId) => MemberstatsDAO.Instance.ChangeGenreOfTrack(trackId, genreId);
    }
}
