using AudioStreamerAPI.Models;
using AudioStreamerAPI.Constants;
using Microsoft.AspNetCore.Identity;

namespace AudioStreamerAPI.DAO
{
    public class PlaylistDAO
    {
        private static PlaylistDAO? _instance;
        private static readonly object _instanceLock = new();

        public static PlaylistDAO Instance
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

        public IEnumerable<Playlist> GetPlaylists()
        {
            IEnumerable<Playlist>? playlists;
            try
            {
                var context = new fsnvdezgContext();
                playlists = context.Playlists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlists;
        }

        public IEnumerable<Playlist> GetPlaylistsFromUser(int id)
        {
            IEnumerable<Playlist>? playlists;
            try
            {
                var context = new fsnvdezgContext();
                playlists = context.Playlists.Where(p => p.MemberId == id).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlists;
        }

        public IEnumerable<Track> GetTracksFromPlaylist(int id)
        {
            List<Track>? tracks = new();
            var playlist = GetPlaylist(id);
            if (playlist != null && playlist.TracksIds!.Length != 0)
            {
                foreach (var tId in playlist.TracksIds)
                {
                    var track = TrackDAO.Instance.GetTrack(tId);
                    if (track != null)
                    {
                        tracks.Add(track);
                    }
                }
            }
            return tracks;
        }

        public IEnumerable<Playlist> SearchPlaylists(string name)
        {
            IEnumerable<Playlist>? playlists;
            try
            {
                var context = new fsnvdezgContext();
                playlists = context.Playlists.Where(p => p.Name.Contains(name.Trim())).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlists;
        }

        public Playlist? GetPlaylist(int id)
        {
            Playlist? playlist;
            try
            {
                var context = new fsnvdezgContext();
                playlist = context.Playlists.FirstOrDefault(p => p.PlaylistId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public Playlist? GetPlaylistFromUser(int userId, int playlistId)
        {
            Playlist? playlist;
            try
            {
                var context = new fsnvdezgContext();
                playlist = context.Playlists.FirstOrDefault(p => p.MemberId == userId && p.PlaylistId == playlistId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public Playlist? GetPlaylistFromUser(int userId, string name)
        {
            Playlist? playlist;
            try
            {
                var context = new fsnvdezgContext();
                playlist = context.Playlists.FirstOrDefault(p => p.MemberId == userId && p.Name.Equals(name.Trim()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public IEnumerable<Playlist> GetPlaylists(string name)
        {
            IEnumerable<Playlist>? playlist;
            try
            {
                var context = new fsnvdezgContext();
                playlist = context.Playlists.Where(p => p.Name.Equals(name.Trim())).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public OperationalStatus AddPlaylist(Playlist playlist)
        {
            Playlist? playlistHasId = GetPlaylistFromUser(playlist.MemberId, playlist.Name);
            if (playlistHasId == null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    /*Playlist p = new()
                    {
                        MemberId = playlist.MemberId,
                        Name = playlist.Name,
                        Description = playlist.Description,
                        TracksIds = playlist.TracksIds,
                    };*/

                    context.Playlists.Add(playlist);
                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Created,
                        Message = "Successfully added playlist to user's directory.",
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.Conflict,
                Message = "Playlist already exists.",
            };
        }

        public OperationalStatus UpdatePlaylist(Playlist playlist)
        {
            Playlist? playlistHasId = GetPlaylist(playlist.PlaylistId);
            if (playlistHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Playlists.Attach(playlistHasId);

                    if (playlist.Name != null)
                    {
                        playlistHasId.Name = playlist.Name;
                    }

                    if (playlist.Description != null)
                    {
                        playlistHasId.Description = playlist.Description;
                    }

                    if (playlist.TracksIds != null)
                    {
                        var newArrayLength = playlist.TracksIds.Length;
                        playlistHasId.TracksIds = new int[newArrayLength];
                        playlist.TracksIds.CopyTo(playlistHasId.TracksIds, 0);
                    }

                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = "Successfully updated user's playlist.",
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = $"Couldn't find playlist with Id: {playlist.PlaylistId}.",
            };
        }

        public OperationalStatus DeletePlaylist(int id)
        {
            Playlist? playlistHasId = GetPlaylist(id);
            if (playlistHasId != null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Playlists.Remove(playlistHasId);
                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = "Successfully deleted playlist from user's directory."
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = $"Couldn't find playlist with Id: {id}.",
            };
        }

        public OperationalStatus AddTrack(int id, int trackId)
        {
            var playlist = GetPlaylist(id);
            var track = TrackDAO.Instance.GetTrack(trackId);

            if (playlist == null || track == null)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Neither playlist with Id: {id} nor track with Id: {trackId} could be found.",
                };
            }

            try
            {
                /*var context = new fsnvdezgContext();
                context.Playlists.Attach(playlist);*/

                if (playlist.TracksIds!.Length == 0)
                {
                    playlist.TracksIds = new int[] { trackId };
                }
                else
                {
                    var tracks = playlist.TracksIds.ToList();
                    tracks.Add(trackId);

                    var newArrayLength = tracks.Count;
                    playlist.TracksIds = new int[newArrayLength];
                    tracks.CopyTo(playlist.TracksIds, 0);
                }

                //context.SaveChanges();
                UpdatePlaylist(playlist);
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = $"Successfully added track to playlist with Id: {id}.",
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public OperationalStatus RemoveTrack(int id, int trackId)
        {
            var playlist = GetPlaylist(id);
            var track = TrackDAO.Instance.GetTrack(trackId);

            if (playlist == null || track == null)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Neither playlist with Id: {id} nor track with Id: {trackId} could be found.",
                };
            }

            try
            {
                /*var context = new fsnvdezgContext();
                context.Playlists.Attach(playlist);*/

                if (playlist.TracksIds!.Length == 0)
                {
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = $"The playlist is empty, there's nothing to remove.",
                    };
                }
                else
                {
                    var tracks = playlist.TracksIds.ToList();
                    tracks.Remove(trackId);

                    var newArrayLength = tracks.Count;
                    playlist.TracksIds = new int[newArrayLength];
                    tracks.CopyTo(playlist.TracksIds, 0);
                }

                //context.SaveChanges();
                UpdatePlaylist(playlist);
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = $"Successfully removed track from playlist with Id: {id}.",
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
