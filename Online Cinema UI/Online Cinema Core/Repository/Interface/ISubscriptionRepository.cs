using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface ISubscriptionRepository : IRepositoryBase<Subscription>
    {
        Task<IEnumerable<Subscription>> GetAllSubscriptionAsync();
        Task<Subscription> GetSubscriptionByIdAsync(int Id);
        Task<IEnumerable<Subscription>> GetSubscriptionByConditionAsync(Expression<Func<Subscription, bool>> predicate);
        Task CreateSubscriptionAsync(Subscription subscription);
        Task UpdateSubscriptionAsync(Subscription subscription);
        Task RemoveSubscriptionAsync(Subscription subscription);
    }
}
//Task<IEnumerable<Address>> GetAll Async();
//Task<Address> Get ByIdAsync(int Id);
//Task<IEnumerable<Address>> Get ByConditionAsync(Expression<Func<Address, bool>> predicate);
//void Create(Address address);
//void Update(Address address);
//void Remove(Address address);