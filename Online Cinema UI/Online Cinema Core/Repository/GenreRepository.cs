using Microsoft.EntityFrameworkCore;
using Online_Cinema_Core.Context;
using Online_Cinema_Core.Repository.Interface;
using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(OnlineCinemaContext context) : base(context) { }

        public void CreateGenre(Genre genre)
        {
            Create(genre);
        }

        public async Task<IEnumerable<Genre>> GetAllGenreAsync()
        {
            return await FindAll().AsNoTracking().Include(x => x.Movies).ToListAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenreByConditionAsync(Expression<Func<Genre, bool>> predicate)
        {
            return await FindByCondition(predicate).Include(x => x.Movies).ToListAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).Include(x=>x.Movies).FirstOrDefaultAsync();
        }
        
        public void RemoveGenre(Genre genre)
        {
            Delete(genre);
        }

        public void UpdateGenre(Genre genre)
        {
            Update(genre);
        }
    }
}
