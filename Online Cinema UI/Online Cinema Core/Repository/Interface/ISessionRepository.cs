﻿using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface ISessionRepository : IRepositoryBase<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionAsync();
        Task<Session> GetSessionByIdAsync(int Id);
        Task<IEnumerable<Session>> GetSessionByConditionAsync(Expression<Func<Session, bool>> predicate);
        void CreateSession(Session session);
        void UpdateSession(Session session);
        void RemoveSession(Session session);
    }
}