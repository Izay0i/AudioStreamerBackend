using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface ICaptionRepository
    {
        IEnumerable<Closedcaption> GetClosedcaptions();
        Closedcaption? GetClosedcaption(int id);
        Closedcaption? GetClosedcaptionFromTrackId(int tId);
        OperationalStatus AddClosedcaption(Closedcaption closedcaption);
        OperationalStatus UpdateClosedcaption(Closedcaption closedcaption);
        OperationalStatus DeleteClosedcaption(int id);
    }
}
