using AudioStreamerAPI.DAO;
using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        public IEnumerable<Genre> GetGenres() => GenreDAO.Instance.GetGenres();
        public OperationalStatus AddGenre(Genre genre) => GenreDAO.Instance.AddGenre(genre);
        public OperationalStatus UpdateGenre(Genre genre) => GenreDAO.Instance.UpdateGenre(genre);
        public OperationalStatus DeleteGenre(int id) => GenreDAO.Instance.DeleteGenre(id);
    }
}
