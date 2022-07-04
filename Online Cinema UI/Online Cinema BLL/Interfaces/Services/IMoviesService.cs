using Online_Cinema_Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Interfaces.Services
{
    public interface IMoviesService
    {
        Task<IDictionary<int, string>> GetDictionaryMoviesAsync();
        Task<Movie> GetMovieAsync(int movieId);
        Task AddMovieAsync(Movie movie, string genre);
        Task ChangeMovieAsync(Movie movie, string genre);
    }
}
