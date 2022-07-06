using System;
using System.ComponentModel.DataAnnotations;

namespace Online_Cinema_Models.View
{
    public class SessionViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }


        [Required, Display(Name = "Movie")]
        public int MovieId { get; set; }


        [Required, Display(Name = "Cinema Room")]
        public int CinemaRoomId { get; set; }


        [Display(Name = "Is Removed?")]
        public bool IsRemoved { get; set; }
    }
}
