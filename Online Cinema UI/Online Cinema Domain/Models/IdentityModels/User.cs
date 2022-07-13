using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Cinema_Domain.Models.IdentityModels
{
    public class User : IdentityUser<Guid>
    {
        public byte[] Photo { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? Birthday { get; set; }
        public Room Room { get; set; }

        public int? SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public IList<Comment> Comments { get; set; }

        public User()
        {
            Comments = new List<Comment>();
            Room = new Room();
            Photo = new byte[] { };
        }
    }
}