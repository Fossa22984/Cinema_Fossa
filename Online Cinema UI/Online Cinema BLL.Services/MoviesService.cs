using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services
{
    public class MoviesService : IMoviesService
    {
        private IUnitOfWork _unit;
        public MoviesService() { }
        public MoviesService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<IDictionary<int, string>> GetDictionaryMoviesAsync()
        {
            var listMovies = await _unit.Movie.GetAllMovieAsync();
            var dictionary = new Dictionary<int, string>();
            foreach (var item in listMovies)
                dictionary.Add(item.Id, $"{item.MovieTitle} (#{item.Id})");
            return dictionary;
        }
        public async Task<Movie> GetMovieAsync(int movieId) => await _unit.Movie.GetMovieByIdAsync(movieId);
        public async Task AddMovieAsync(Movie movie, string genre)
        {
            try
            {
                if (genre != null)
                {
                    var res = (await _unit.Genre.GetAllGenreAsync()).Where(x => genre.Contains(x.GenreName, StringComparison.OrdinalIgnoreCase)).ToList();
                    movie.Genres = res;
                }
                await _unit.Movie.CreateAsync(movie);
                await _unit.SaveAsync();
            }
            catch (Exception)
            {
            }
        }
        public async Task ChangeMovieAsync(Movie movie, string genre)
        {
            try
            {
                if (genre != null)
                {
                    var res = (await _unit.Genre.GetAllGenreAsync()).Where(x => genre.Contains(x.GenreName, StringComparison.OrdinalIgnoreCase));
                    movie.Genres = res.ToList();
                }
                await _unit.Movie.UpdateAsync(movie);
                await _unit.SaveAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
