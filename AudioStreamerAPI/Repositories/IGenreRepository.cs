using AudioStreamerAPI.Models;

namespace AudioStreamerAPI.Repositories
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
        OperationalStatus AddGenre(Genre genre);
        OperationalStatus UpdateGenre(Genre genre);
        OperationalStatus DeleteGenre(int id);
    }
}
