using Microsoft.EntityFrameworkCore;
using Online_Cinema_Core.Context;
using Online_Cinema_Core.Repository.Interface;
using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository
{
    public class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {
        public MovieRepository(OnlineCinemaContext context) : base(context) { }

        public void CreateMovie(Movie movie)
        {
           Create(movie);
        }

        public async Task<IEnumerable<Movie>> GetAllMovieAsync()
        {
            return await FindAll().Include(x => x.Genre).ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMovieByConditionAsync(Expression<Func<Movie, bool>> predicate)
        {
            return await FindByCondition(predicate).Include(x=>x.Genre).ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).Include(x => x.Genre).FirstOrDefaultAsync();
        }

        public void RemoveMovie(Movie movie)
        {
            Delete(movie);
        }

        public void UpdateMovie(Movie movie)
        {
            Update(movie);
        }
    }
}
