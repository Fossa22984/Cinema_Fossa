﻿using Online_Cinema_BLL.Interfaces.Services;
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
                _unit.Movie.Create(movie);
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
                _unit.Movie.Update(movie);
                await _unit.SaveAsync();
            }
            catch (Exception)
            {
            }
        }

        //private readonly OnlineCinemaContext _context;

        //public MoviesService(OnlineCinemaContext context)
        //{
        //    this._context = context;
        //}

        //public IList<Movie> GetMovies() => _context.Movies.Include(x => x.Genre).ToList();
        //public IList<Movie> GetMovies(int pageNumber)
        //{
        //    return _context.Movies.Include(x => x.Genre).Where(x => x.Id >= (pageNumber * 12 - 11) && x.Id <= (pageNumber * 12)).ToList();
        //}

        //public IList<Movie> GetMovies(int pageNumber, string search)
        //{
        //    var listFilms = _context.Movies.Include(x => x.Genre).AsEnumerable().Where(x => x.MovieTitle.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        //    if (listFilms.Count > 12)
        //        return listFilms.GetRange(pageNumber * 12 - 11, 12);

        //    return listFilms;
        //}
    }
}
