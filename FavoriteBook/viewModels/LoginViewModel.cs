using System.ComponentModel.DataAnnotations;

namespace FavoriteBook.viewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Please enter your Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Please enter your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
