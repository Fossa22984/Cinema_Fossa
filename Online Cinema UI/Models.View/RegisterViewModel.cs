using System;
using System.ComponentModel.DataAnnotations;

namespace Online_Cinema_Models.View
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password)), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required, Display(Name = "Date of Birth")]
        public DateTime Birthday { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
