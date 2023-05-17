using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.DAO
{
    public class TrackDAO
    {
        private static TrackDAO? _instance;
        private static readonly object _instanceLock = new();

        public static TrackDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new();
                    }
                    return _instance;
                }
            }
        }

        public IEnumerable<Track> GetTracks()
        {
            List<Track>? tracks;
            try
            {
                var context = new fsnvdezgContext();
                tracks = context.Tracks.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tracks;
        }

        public IEnumerable<Track> SearchTracks(string keyword)
        {
            List<Track>? tracks;
            try
            {
                var context = new fsnvdezgContext();
                var filteredTracks = new List<Track>();
                filteredTracks.AddRange(context.Tracks.Where(t => t.TrackName.Contains(keyword.Trim())));
                filteredTracks.AddRange(context.Tracks.Where(t => t.Tags!.Contains(keyword.Trim())));
                tracks = filteredTracks.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tracks;
        }

        public Track? GetTrack(int id)
        {
            Track? track;
            try
            {
                var context = new fsnvdezgContext();
                track = context.Tracks.SingleOrDefault(t => t.TrackId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return track;
        }

        public Track? GetTrack(string name, string[]? tags)
        {
            Track? track;
            try
            {
                var context = new fsnvdezgContext();
                if (tags != null)
                {
                    var tracks = context.Tracks.Where(t => t.Tags == tags).ToList();
                    return track = tracks.FirstOrDefault(t => t.TrackName == name);
                }
                return track = context.Tracks.FirstOrDefault(t => t.TrackName == name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public OperationalStatus AddTrack(Track track)
        {
            Track? trackHasId = GetTrack(track.TrackId);
            if (trackHasId != null)
            {
                return OperationalStatus.FAILURE;
            }
            else
            {
                try
                {
                    var context = new fsnvdezgContext();
                    Track t = new()
                    {
                        TrackName = track.TrackName,
                        Description = track.Description,
                        Url = track.Url,
                        Thumbnail = track.Thumbnail,
                        AuthorsIds = track.AuthorsIds,
                        Tags = track.Tags,
                    };

                    context.Tracks.Add(t);
                    context.SaveChanges();
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public OperationalStatus UpdateTrack(Track track)
        {
            Track? trackHasId = GetTrack(track.TrackId);
            if (trackHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Tracks.Attach(trackHasId);

                    if (track.TrackName != null)
                    {
                        trackHasId.TrackName = track.TrackName;
                    }

                    if (track.Description != null)
                    {
                        trackHasId.Description = track.Description;
                    }

                    if (track.Thumbnail != null)
                    {
                        trackHasId.Thumbnail = track.Thumbnail;
                    }

                    if (track.Tags != null)
                    {
                        var newArrayLength = track.Tags.Length;
                        trackHasId.Tags = new string[newArrayLength];
                        track.Tags.CopyTo(trackHasId.Tags, 0);
                    }

                    context.SaveChanges();
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return OperationalStatus.FAILURE;
        }

        public OperationalStatus DeleteTrack(int id)
        {
            Track? trackHasId = GetTrack(id);
            if (trackHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Tracks.Remove(trackHasId);
                    context.SaveChanges();
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return OperationalStatus.FAILURE;
        }
    }
}
