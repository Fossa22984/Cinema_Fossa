using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

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
        public bool IsOpen { get; set; }

        public Guid OwnerId { get; set; }
        public string Owner { get; set; }

        public int? MovieId { get; set; }

        [Display(Name = "Is Removed?")]
        public bool IsRemoved { get; set; }
        public MovieViewModel Movie { get; set; }
    }
}
