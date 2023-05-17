﻿using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Models;

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
                    if (_instance == null)
                    {
                        _instance = new();
                    }
                    return _instance;
                }
            }
        }

        public IEnumerable<Playlist> GetPlaylists()
        {
            List<Playlist>? playlists;
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
            List<Playlist>? playlists;
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
            List<Playlist>? playlists;
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

        public Playlist? GetPlaylist(string name)
        {
            Playlist? playlist;
            try
            {
                var context = new fsnvdezgContext();
                playlist = context.Playlists.FirstOrDefault(p => p.Name.Equals(name.Trim()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public OperationalStatus AddPlaylist(Playlist playlist)
        {
            Playlist? playlistHasId = GetPlaylist(playlist.PlaylistId);
            if (playlistHasId != null)
            {
                return OperationalStatus.FAILURE;
            }
            else
            {
                try
                {
                    var context = new fsnvdezgContext();
                    Playlist p = new()
                    {
                        MemberId = playlist.MemberId,
                        Name = playlist.Name,
                        Description = playlist.Description,
                        TracksIds = playlist.TracksIds,
                    };

                    context.Playlists.Add(p);
                    context.SaveChanges();
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
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
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return OperationalStatus.FAILURE;
            }
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
                    return OperationalStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return OperationalStatus.FAILURE;
            }
        }

        public OperationalStatus AddTrack(int id, int trackId)
        {
            var playlist = GetPlaylist(id);
            var track = TrackDAO.Instance.GetTrack(trackId);

            if (playlist == null || track == null)
            {
                return OperationalStatus.FAILURE;
            }

            try
            {
                var context = new fsnvdezgContext();
                context.Playlists.Attach(playlist);

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

                context.SaveChanges();
                return OperationalStatus.SUCCESS;
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
                return OperationalStatus.FAILURE;
            }

            try
            {
                var context = new fsnvdezgContext();
                context.Playlists.Attach(playlist);

                if (playlist.TracksIds!.Length == 0)
                {
                    return OperationalStatus.FAILURE;
                }
                else
                {
                    var tracks = playlist.TracksIds.ToList();
                    tracks.Remove(trackId);

                    var newArrayLength = tracks.Count;
                    playlist.TracksIds = new int[newArrayLength];
                    tracks.CopyTo(playlist.TracksIds, 0);
                }

                context.SaveChanges();
                return OperationalStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}