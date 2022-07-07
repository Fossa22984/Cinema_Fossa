using Microsoft.AspNetCore.Http;
using Online_Cinema_Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Cinema_Models.View
{
    public class CinemaRoomViewModel
    {
        public int Id { get; set; }
        public byte[] CinemaRoomImage { get; set; }

        [/*Required,*/ Display(Name = "Cinema Room Image")]
        public IFormFile ImageFile { get; set; }

        [Required, Display(Name = "Cinema Room Name")]
        public string CinemaRoomName { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Is Removed?")]
        public bool IsRemoved { get; set; }
        public IList<SessionViewModel> Sessions { get; set; }

        public CinemaRoomViewModel()
        {
            Sessions = new List<SessionViewModel>();
        }

    }
}
