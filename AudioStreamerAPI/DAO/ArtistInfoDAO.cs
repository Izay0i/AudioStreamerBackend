using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.DAO
{
    public class ArtistInfoDAO
    {
        private static ArtistInfoDAO? _instance;
        private static readonly object _instanceLock = new();

        public static ArtistInfoDAO Instance
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

        public IEnumerable<Artistinfo> GetArtists()
        {
            IEnumerable<Artistinfo> artists;
            try
            {
                var context = new fsnvdezgContext();
                artists = context.Artistinfos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return artists;
        }

        public IEnumerable<Track> GetTracks(int id)
        {
            IEnumerable<Track> tracks;
            try
            {
                var context = new fsnvdezgContext();
                tracks = context.Tracks.Where(t => t.ArtistinfoId == id).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tracks;
        }

        public Artistinfo? GetArtist(int id)
        {
            Artistinfo? artist;
            try
            {
                var context = new fsnvdezgContext();
                artist = context.Artistinfos.SingleOrDefault(a => a.ArtistinfoId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return artist;
        }

        public Artistinfo? GetArtist(string name)
        {
            Artistinfo? artist;
            try
            {
                var context = new fsnvdezgContext();
                artist = context.Artistinfos.SingleOrDefault(a => a.ArtistName.Trim().ToLower().Equals(name.Trim().ToLower()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return artist;
        }

        public string GetArtistName(int id)
        {
            string artistName;
            try
            {
                var context = new fsnvdezgContext();
                artistName = context.Artistinfos.Where(a => a.ArtistinfoId == id).Select(a => a.ArtistName).SingleOrDefault() ?? "";
                //artistName = artists.FirstOrDefault() ?? "";
            }
            catch (Exception ex)
            {
                artistName = ex.Message;
            }
            return artistName;
        }

        public OperationalStatus AddArtist(Artistinfo artistinfo)
        {
            try
            {
                var artist = GetArtist(artistinfo.ArtistName);
                if (artist != null)
                {
                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.Conflict,
                        Message = "Artist already exists.",
                    };
                }

                var context = new fsnvdezgContext();
                context.Artistinfos.Add(artistinfo);
                context.SaveChanges();

                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Created,
                    Message = "Successfully added artist.",
                    Objects = new object[] { artistinfo.ArtistinfoId },
                };
            }
            catch (Exception ex)
            {
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                };
            }
        }

        public OperationalStatus UpdateArtist(Artistinfo artistinfo)
        {
            try
            {
                var artist = GetArtist(artistinfo.ArtistinfoId);
                if (artist == null)
                {
                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.NotFound,
                        Message = "Artist not found.",
                    };
                }

                var context = new fsnvdezgContext();
                context.Artistinfos.Attach(artist);
                artist.ArtistName = artistinfo.ArtistName;
                artist.Description = artistinfo.Description;
                artist.Avatar = artistinfo.Avatar;
                artist.MainSiteAddress = artistinfo.MainSiteAddress;
                context.SaveChanges();

                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = "Successfully updated artist.",
                    Objects = new object[] { artistinfo.ArtistinfoId },
                };
            }
            catch (Exception ex)
            {
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                };
            }
        }

        public OperationalStatus DeleteArtist(int id)
        {
            try
            {
                var artist = GetArtist(id);
                if (artist == null)
                {
                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.NotFound,
                        Message = "Artist not found.",
                    };
                }

                var context = new fsnvdezgContext();
                context.Artistinfos.Remove(artist);
                context.SaveChanges();

                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = "Successfully deleted artist.",
                };
            }
            catch (Exception ex)
            {
                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                };
            }
        }
    }
}
