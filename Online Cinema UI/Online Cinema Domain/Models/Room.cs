using Online_Cinema_Domain.Models.IdentityModels;
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

        public IList<User> Participants { get; set; }

        public Room()
        {
            Participants = new List<User>();
        }

        public bool Remote { get; set; }
    }
}