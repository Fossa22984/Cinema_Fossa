using Online_Cinema_Domain.Models.IdentityModels;
using System.Collections.Generic;

namespace Online_Cinema_Domain.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string SubscriptionName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public IList<User> Users { get; set; }

        public Subscription()
        {
            Users = new List<User>();
        }

        public bool Remote { get; set; }
    }
}