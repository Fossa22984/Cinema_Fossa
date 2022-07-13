using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface IGenreRepository : IRepositoryBase<Genre>
    {
        Task<IEnumerable<Genre>> GetAllGenreAsync();
        Task<Genre> GetGenreByIdAsync(int Id);
        Task<IEnumerable<Genre>> GetGenreByConditionAsync(Expression<Func<Genre, bool>> predicate);
        Task CreateGenreAsync(Genre genre);
        Task UpdateGenreAsync(Genre genre);
        Task RemoveGenreAsync(Genre genre);
    }
}
