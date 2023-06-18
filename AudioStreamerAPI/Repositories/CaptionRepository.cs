using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class CaptionRepository : ICaptionRepository
    {
        public IEnumerable<Closedcaption> GetClosedcaptions() => ClosedcaptionDAO.Instance.GetClosedcaptions();
        public Closedcaption? GetClosedcaption(int id) => ClosedcaptionDAO.Instance.GetClosedcaption(id);
        public Closedcaption? GetClosedcaptionFromTrackId(int tId) => ClosedcaptionDAO.Instance.GetClosedcaptionFromTrackId(tId);
        public OperationalStatus AddClosedcaption(Closedcaption opencaption) => ClosedcaptionDAO.Instance.AddClosedcaption(opencaption);
        public OperationalStatus UpdateClosedcaption(Closedcaption opencaption) => ClosedcaptionDAO.Instance.UpdateClosedcaption(opencaption);
        public OperationalStatus DeleteClosedcaption(int id) => ClosedcaptionDAO.Instance.DeleteClosedcaption(id);
    }
}
