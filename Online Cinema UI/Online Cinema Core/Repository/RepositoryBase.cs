using Online_Cinema_Core.Context;
using Online_Cinema_Core.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected OnlineCinemaContext context { get; set; }
        public RepositoryBase(OnlineCinemaContext context) { this.context = context; }

        public async Task Create(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public Task Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public IQueryable<T> FindAll()
        {
            return context.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>()
                .Where(predicate);
        }

        public Task Update(T entity)
        {
            context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
    }
}
