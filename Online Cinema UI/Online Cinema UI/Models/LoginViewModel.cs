using System.ComponentModel.DataAnnotations;

namespace Online_Cinema_UI.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
