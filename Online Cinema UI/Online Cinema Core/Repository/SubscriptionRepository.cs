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
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(OnlineCinemaContext context) : base(context) { }

        public void CreateSubscription(Subscription subscription)
        {
            Create(subscription);
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionByConditionAsync(Expression<Func<Subscription, bool>> predicate)
        {
            return await FindByCondition(predicate).ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public void RemoveSubscription(Subscription subscription)
        {
            Delete(subscription);
        }

        public void UpdateSubscription(Subscription subscription)
        {
            Update(subscription);
        }
    }
}
