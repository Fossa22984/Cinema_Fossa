using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
