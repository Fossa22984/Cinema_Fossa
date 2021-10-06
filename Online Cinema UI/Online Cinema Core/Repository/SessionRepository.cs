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
    public class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(OnlineCinemaContext context) : base(context) { }

        public void CreateSession(Session session)
        {
            Create(session);
        }

        public async Task<IEnumerable<Session>> GetAllSessionAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionByConditionAsync(Expression<Func<Session, bool>> predicate)
        {
            return await FindByCondition(predicate).ToListAsync();
        }

        public async Task<Session> GetSessionByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public void RemoveSession(Session session)
        {
            Delete(session);
        }

        public void UpdateSession(Session session)
        {
            Update(session);
        }
    }
}
