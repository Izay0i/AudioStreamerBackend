using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.DAO
{
    public class GenreDAO
    {
        private static GenreDAO? _instance;
        private static readonly object _instanceLock = new();

        public static GenreDAO Instance
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

        public IEnumerable<Genre> GetGenres()
        {
            IEnumerable<Genre> genres;
            try
            {
                var context = new fsnvdezgContext();
                genres = context.Genres.OrderBy(g => g.GenreId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return genres;
        }

        public OperationalStatus AddGenre(Genre genre)
        {
            try
            {
                var context = new fsnvdezgContext();
                context.Genres.Add(genre);
                context.SaveChanges();

                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Created,
                    Message = "New genre successfully added.",
                    Objects = new object[] { genre.GenreId },
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

        public OperationalStatus UpdateGenre(Genre genre)
        {
            try
            {
                var context = new fsnvdezgContext();
                var g = context.Genres.SingleOrDefault(g => g.GenreId == genre.GenreId);
                if (g == null)
                {
                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.NotFound,
                        Message = "Genre not found.",
                    };
                }
                context.Genres.Attach(g);
                g.GenreName = genre.GenreName;
                context.SaveChanges();

                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = "Successfully updated genre.",
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

        public OperationalStatus DeleteGenre(int id)
        {
            try
            {
                var context = new fsnvdezgContext();
                var genre = context.Genres.SingleOrDefault(g => g.GenreId == id);
                if (genre == null)
                {
                    return new OperationalStatus
                    {
                        StatusCode = Constants.OperationalStatusEnums.NotFound,
                        Message = "Genre not found.",
                    };
                }
                context.Genres.Remove(genre);
                context.SaveChanges();

                return new OperationalStatus
                {
                    StatusCode = Constants.OperationalStatusEnums.Ok,
                    Message = "Successfully removed genre.",
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
