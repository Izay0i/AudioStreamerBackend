using AudioStreamerAPI.Models;
using AudioStreamerAPI.Constants;
using System.Reflection.Metadata.Ecma335;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;

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
                    _instance ??= new();
                    return _instance;
                }
            }
        }

        public IEnumerable<Track> GetTracks()
        {
            IEnumerable<Track>? tracks;
            try
            {
                var context = new fsnvdezgContext();
                tracks = context.Tracks.OrderByDescending(t => t.TrackId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tracks;
        }

        public IEnumerable<Track> GetTracksWithTheMostViewsOfTheDay(int n)
        {
            IEnumerable<Track>? tracks;
            try
            {
                var context = new fsnvdezgContext();
                tracks = context.Tracks
                    .OrderByDescending(track => track.ViewCountsPerDay)
                    .ThenByDescending(track => track.TrackId)
                    .Take(n);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tracks;
        }

        public IEnumerable<Track> GetTracksFromUserId(int uId)
        {
            IEnumerable<Track>? tracks;
            try
            {
                var context = new fsnvdezgContext();
                tracks = context.Tracks.Where(track => track.MemberId == uId).OrderByDescending(t => t.TrackId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tracks;
        }

        public IEnumerable<Track> SearchTracks(string keyword)
        {
            IEnumerable<Track>? tracks;
            try
            {
                //TODO: REFACTOR
                var context = new fsnvdezgContext();
                var filteredTracks = new List<Track>();
                filteredTracks.AddRange(context.Tracks.Where(t => t.ArtistName.Contains(keyword)));
                filteredTracks.AddRange(context.Tracks.Where(t => t.TrackName.Contains(keyword.Trim())));
                filteredTracks.AddRange(context.Tracks.Where(t => t.Tags!.Contains(keyword.Trim())));
                tracks = filteredTracks.Distinct()
                    .OrderByDescending(t => t.TrackId)
                    .ThenByDescending(t => t.ViewCountsPerDay)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tracks;
        }

        public IEnumerable<int> GetRandomTrackIds(int limit)
        {
            List<int> trackIds = new();
            try
            {
                var context = new fsnvdezgContext();
                trackIds = context.Tracks.Select(track => track.TrackId).OrderBy(t => Guid.NewGuid()).Take(limit).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return trackIds;
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
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Conflict,
                    Message = $"Track wiht Id: {trackHasId.TrackId} already exists.",
                };
            }
            else
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Tracks.Add(track);
                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Created,
                        Message = "Successfully added track to user's directory.",
                    };
                }
                catch (Exception ex)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = ex.Message,
                    };
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

                    if (track.ArtistName != null)
                    {
                        trackHasId.ArtistName = track.ArtistName;
                    }

                    if (track.Description != null)
                    {
                        trackHasId.Description = track.Description;
                    }

                    if (track.Thumbnail != null)
                    {
                        trackHasId.Thumbnail = track.Thumbnail;
                    }

                    if (track.Tags!.Length == 0)
                    {
                        if (trackHasId.Tags!.Length == 0)
                        {
                            trackHasId.Tags = Array.Empty<string>();
                        }
                    }
                    else
                    {
                        var newArrayLength = track.Tags.Length;
                        trackHasId.Tags = new string[newArrayLength];
                        track.Tags.CopyTo(trackHasId.Tags, 0);
                    }

                    trackHasId.HasCaptions = track.HasCaptions;
                    trackHasId.CaptionsLength = track.CaptionsLength;

                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = "Successfully updated track's info.",
                    };
                }
                catch (Exception ex)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = ex.Message,
                    };
                }
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = "Failed to find track.",
            };
        }

        public OperationalStatus IncreaseViewCountsOfTheDay(int tId)
        {
            Track? trackHasId = GetTrack(tId);
            if (trackHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Tracks.Attach(trackHasId);
                    trackHasId.ViewCountsPerDay++;
                    context.SaveChanges();
                    
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = $"Successfully increased view count of track with id: {tId}",
                    };
                }
                catch (Exception ex)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = ex.Message,
                    };
                }
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = $"Couldn't find track with Id: {tId}.",
            };
        }

        public OperationalStatus ResetViewCountsOfAllTracks()
        {
            try
            {
                var context = new fsnvdezgContext();
                //mfw 6.0 doesn't support executeupdate/delete
                context.Database.ExecuteSqlInterpolated($"UPDATE Track SET view_counts_per_day = 0;");

                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = "Successfully reset view counts of every track",
                };
            }
            catch (Exception ex)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                };
            }
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
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = $"Successfully removed track with Id: {id}.",
                    };
                }
                catch (Exception ex)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = ex.Message,
                    };
                }
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = $"Couldn't find track with Id: {id}.",
            };
        }
    }
}
