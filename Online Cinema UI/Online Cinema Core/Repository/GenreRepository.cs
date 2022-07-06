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
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(OnlineCinemaContext context) : base(context) { }

        public async Task CreateGenre(Genre genre)
        {
           await Create(genre);
        }

        public async Task<IEnumerable<Genre>> GetAllGenreAsync()
        {
            return await FindAll().Include(x => x.Movies).ToListAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenreByConditionAsync(Expression<Func<Genre, bool>> predicate)
        {
            return await FindByCondition(predicate).Include(x => x.Movies).ToListAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).Include(x => x.Movies).FirstOrDefaultAsync();
        }

        public async Task RemoveGenre(Genre genre)
        {
           await Delete(genre);
        }

        public async Task UpdateGenre(Genre genre)
        {
           await Update(genre);
        }
    }
}
