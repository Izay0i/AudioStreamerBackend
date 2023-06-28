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
                playlists = context.Playlists.OrderBy(p => p.PlaylistId).ToList();
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
                playlists = context.Playlists
                    .Where(p => p.MemberId == id)
                    .OrderByDescending(p => p.PlaylistId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlists;
        }

        public IEnumerable<Playlist> GetPlaylistsFromUser(int userId, string name)
        {
            IEnumerable<Playlist>? playlists;
            try
            {
                playlists = GetPlaylistsFromUser(userId);
                playlists = playlists
                    .Where(p => p.Name.Contains(name.Trim()))
                    .OrderByDescending(p => p.PlaylistId)
                    .ToList();
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
                        tracks.Insert(0, track);
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
                playlists = context.Playlists
                    .Where(p => p.Name.Contains(name.Trim()))
                    .OrderByDescending (p => p.PlaylistId)
                    .ToList();
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
                playlist = context.Playlists.SingleOrDefault(p => p.PlaylistId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public Playlist? GetPlaylistByIdFromUser(int userId, int playlistId)
        {
            Playlist? playlist;
            try
            {
                var context = new fsnvdezgContext();
                playlist = context.Playlists.SingleOrDefault(p => p.MemberId == userId && p.PlaylistId == playlistId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public Playlist? GetPlaylistByNameFromUser(int userId, string name)
        {
            Playlist? playlist;
            try
            {
                var context = new fsnvdezgContext();
                playlist = context.Playlists.SingleOrDefault(p => p.MemberId == userId && p.Name.Equals(name.Trim()));
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
                playlist = context.Playlists
                    .Where(p => p.Name.Contains(name.Trim()))
                    .OrderByDescending(p => p.PlaylistId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public OperationalStatus AddPlaylist(Playlist playlist)
        {
            Playlist? playlistHasId = GetPlaylistByNameFromUser(playlist.MemberId, playlist.Name);
            if (playlistHasId == null)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Playlists.Add(playlist);
                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Created,
                        Message = "Successfully added playlist to user's directory.",
                        Objects = new object[] { playlist.PlaylistId },
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

                    Playlist? playlistHasName = GetPlaylistByNameFromUser(playlistHasId.MemberId, playlist.Name);
                    if (playlist.Name != null && playlistHasName == null)
                    {
                        playlistHasId.Name = playlist.Name;
                    }

                    if (playlist.Description != null)
                    {
                        playlistHasId.Description = playlist.Description;
                    }
                    
                    playlistHasId.TracksIds = playlist.TracksIds;

                    context.SaveChanges();
                    return new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.Ok,
                        Message = "Successfully updated user's playlist.",
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
                Message = $"Couldn't find playlist with Id: {id}.",
            };
        }

        public OperationalStatus AddTrack(int id, int trackId)
        {
            var playlist = GetPlaylist(id);
            if (playlist == null)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Playlist could not be found.",
                };
            }

            var track = playlist.TracksIds!.Contains(trackId);
            if (track)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Conflict,
                    Message = $"Already added track to playlist.",
                };
            }

            try
            {
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

                return UpdatePlaylist(playlist);
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

        public OperationalStatus RemoveTrack(int id, int trackId)
        {
            var playlist = GetPlaylist(id);
            if (playlist == null)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Playlist could not be found.",
                };
            }

            var track = playlist.TracksIds!.Contains(trackId);
            if (!track)
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Track could not be found.",
                };
            }

            try
            {
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

                return UpdatePlaylist(playlist);
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
}
