using Online_Cinema_Domain.Models.IdentityModels;
using System;
using System.Collections.Generic;

namespace Online_Cinema_Domain.Models
{
    public class Room
    {
        public int Id { get; set; }
        public byte[] RoomImage { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string CreatorName { get; set; }
        public bool IsOpen { get; set; }

        public int? MovieId { get; set; }
        public Movie Movie { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public bool IsRemoved { get; set; }
    }
}