using AudioStreamerAPI.Helpers;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.DAO
{
    public class ClosedcaptionDAO
    {
        private static ClosedcaptionDAO? _instance;
        private static readonly object _instanceLock = new();

        public static ClosedcaptionDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new();
                    return _instance;
                }
            }
        }

        public IEnumerable<Closedcaption> GetClosedcaptions()
        {
            List<Closedcaption>? captions;
            try
            {
                var context = new fsnvdezgContext();
                captions = context.Closedcaptions.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return captions;
        }

        public Closedcaption? GetClosedcaption(int id)
        {
            Closedcaption? closedcaption;
            try
            {
                var context = new fsnvdezgContext();
                closedcaption = context.Closedcaptions.SingleOrDefault(caption => caption.CaptionId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return closedcaption;
        }

        public Closedcaption? GetClosedcaptionFromTrackId(int tId) 
        {
            Closedcaption? closedcaption;
            try
            {
                var context = new fsnvdezgContext();
                closedcaption = context.Closedcaptions.SingleOrDefault(caption => caption.TrackId == tId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return closedcaption;
        }

        public OperationalStatus AddClosedcaption(Closedcaption closedcaption)
        {
            Closedcaption? captionHasId = GetClosedcaptionFromTrackId(closedcaption.TrackId);
            if (captionHasId == null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    Closedcaption caption = new()
                    {
                        TrackId = closedcaption.TrackId,
                        Captions = closedcaption.Captions.IsValidJson() ? closedcaption.Captions : "[]",
                    };

                    context.Closedcaptions.Add(caption);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Created,
                    Message = "Successfully added closed caption.",
                };
            }
            return new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Conflict,
                Message = $"Closed caption already created.",
            };
        }

        public OperationalStatus UpdateClosedcaption(Closedcaption closedcaption)
        {
            Closedcaption? captionHasId = GetClosedcaption(closedcaption.CaptionId);
            if (captionHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Closedcaptions.Attach(captionHasId);
                    captionHasId.Captions = closedcaption.Captions.IsValidJson() ? closedcaption.Captions : captionHasId.Captions;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = "Updated closed caption.",
                };
            }
            return new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Closed caption not found.",
            };
        }

        public OperationalStatus DeleteClosedcaption(int id)
        {
            Closedcaption? captionHasId = GetClosedcaption(id);
            if (captionHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Closedcaptions.Remove(captionHasId);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = "Successfully deleted closed caption.",
                };
            }
            return new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = "Closed caption not found.",
            };
        }
    }
}
