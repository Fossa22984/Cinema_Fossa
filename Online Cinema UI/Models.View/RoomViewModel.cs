using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_Models.View
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public byte[] RoomImage { get; set; }

        [/*Required,*/ Display(Name = "Cinema Room Image")]
        public IFormFile ImageFile { get; set; }

        [Required, Display(Name = "Cinema Room Name")]
        public string RoomName { get; set; }

        [Required]
        public string Description { get; set; }

        public string Password { get; set; }
        public bool IsOpen { get; set; }
        public string Owner { get; set; }

        public int? MovieId { get; set; }

        public RoomViewModel()
        {

        }

        [Display(Name = "Is Removed?")]
        public bool IsRemoved { get; set; }
    }
}
