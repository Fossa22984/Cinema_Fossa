using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface IMovieRepository : IRepositoryBase<Movie>
    {
        Task<IEnumerable<Movie>> GetAllMovieAsync();
        Task<Movie> GetMovieByIdAsync(int Id);
        Task<IEnumerable<Movie>> GetMovieByConditionAsync(Expression<Func<Movie, bool>> predicate);
        Task CreateMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void RemoveMovie(Movie movie);
    }
}
