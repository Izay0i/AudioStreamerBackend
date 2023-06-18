using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface IMemberstatsRepository
    {
        OperationalStatus GetStats(int trackId);
        Memberstat? GetStats(int userId, int trackId);
        OperationalStatus AddStats(Memberstat memberstat);
        OperationalStatus UpdateStats(Memberstat memberstat);
        OperationalStatus DeleteStats(int userId, int trackId);
        OperationalStatus DeleteStatsOfUser(int userId);
        OperationalStatus DeleteStatsOfTrack(int trackId);
    }
}
