using Online_Cinema_Domain.Models.IdentityModels;
using System.Collections.Generic;

namespace Online_Cinema_Domain.Models
{
    public class CinemaRoom
    {
        public int Id { get; set; }
        public byte[] CinemaRoomImage { get; set; }
        public string CinemaRoomName { get; set; }
        public string Description { get; set; }

        public IList<Session> Sessions { get; set; }
        public IList<User> ViewersList { get; set; }

        public CinemaRoom()
        {
            Sessions = new List<Session>();
            ViewersList = new List<User>();
        }

        public bool Remote { get; set; }
    }
}
